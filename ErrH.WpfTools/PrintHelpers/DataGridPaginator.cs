using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using ErrH.Tools.Extensions;
using ErrH.WpfTools.Extensions;

namespace ErrH.WpfTools.PrintHelpers
{
    public class DataGridPaginator : DocumentPaginator
    {
        private Collection<ColumnDefinition> _tableColumnDefinitions;
        private DataGrid  _documentSource;
        private double    _avgRowHeight;
        private double    _availableHeight;
        private int       _rowsPerPage;
        private int       _pageCount;
        private double    _rowCount;

        public string     AppTitle                   { get; set; }
        public string     HeaderLeftText             { get; set; }
        public string     HeaderRightText            { get; set; }
        public Thickness  PageMargin                 { get; set; }
        public Style      HeaderLeftStyle            { get; set; }
        public Style      HeaderRightStyle           { get; set; }
        public Style      AlternatingRowBorderStyle  { get; set; }
        public Style      FooterLeftStyle            { get; set; }
        public Style      FooterCenterStyle          { get; set; }
        public Style      FooterRightStyle           { get; set; }
        public Style      TableCellTextStyle         { get; set; }
        public Style      TableHeaderTextStyle       { get; set; }
        public Style      TableHeaderBorderStyle     { get; set; }
        public Style      GridContainerStyle         { get; set; }

        public override Size  PageSize                  { get; set; }
        public override bool  IsPageCountValid          => true;
        public override int   PageCount                 => _pageCount;
        public override IDocumentPaginatorSource Source => null;



        public DataGridPaginator(DataGrid documentSource,
                                 PrintDialog printDialog)
        {
            _documentSource         = documentSource;
            _tableColumnDefinitions = new Collection<ColumnDefinition>();
            this.PageMargin         = new Thickness(30, 30, 30, 30);
            this.PageSize           = new Size(printDialog.PrintableAreaWidth, 
                                               printDialog.PrintableAreaHeight);
            if (_documentSource != null)
            {
                _rowCount = CountRows(_documentSource.ItemsSource);
                MeasureElements();
            }

            HeaderLeftStyle      = DefaultHeaderLeftStyle();
            FooterCenterStyle    = DefaultFooterCenterStyle();
            TableHeaderTextStyle = DefaultTableHeaderTextStyle();
            TableCellTextStyle   = DefaultTableCellTextStyle();
        }


        private Style DefaultTableCellTextStyle()
        {
            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis));
            style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            //style.Setters.Add(new Setter(FrameworkElement.HeightProperty, 100D));
            return style;
        }


        private Style DefaultTableHeaderTextStyle()
        {
            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.SemiBold));
            style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 10.5D));
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
            return style;
        }


        private Style DefaultHeaderLeftStyle()
        {
            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.DemiBold));
            style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14D));
            return style;
        }


        private Style DefaultFooterCenterStyle()
        {
            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.FontStyleProperty, FontStyles.Italic));
            return style;
        }



        #region Public Methods

        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPage page = null;
            List<object> itemsSource = new List<object>();

            ICollectionView viewSource = CollectionViewSource.GetDefaultView(_documentSource.ItemsSource);

            if (viewSource != null)
            {
                foreach (object item in viewSource)
                    itemsSource.Add(item);
            }

            if (itemsSource != null)
            {
                int rowIndex = 1;
                int startPos = pageNumber * _rowsPerPage;
                int endPos = startPos + _rowsPerPage;

                //Create a new grid
                Grid tableGrid = CreateTable(true) as Grid;

                for (int index = startPos; index < endPos && index < itemsSource.Count; index++)
                {
                    Console.WriteLine("Adding: " + index);

                    if (rowIndex > 0)
                    {
                        object item = itemsSource[index];
                        int columnIndex = 0;

                        if (_documentSource.Columns != null)
                        {
                            foreach (DataGridColumn column in _documentSource.Columns)
                            {
                                if (column.Visibility == Visibility.Visible)
                                {
                                    AddTableCell(tableGrid, column, item, columnIndex, rowIndex);
                                    columnIndex++;
                                }
                            }
                        }

                        if (this.AlternatingRowBorderStyle != null && rowIndex % 2 == 0)
                        {
                            Border alernatingRowBorder = new Border();

                            alernatingRowBorder.Style = this.AlternatingRowBorderStyle;
                            alernatingRowBorder.SetValue(Grid.RowProperty, rowIndex);
                            alernatingRowBorder.SetValue(Grid.ColumnSpanProperty, columnIndex);
                            alernatingRowBorder.SetValue(Grid.ZIndexProperty, -1);
                            tableGrid.Children.Add(alernatingRowBorder);
                        }
                    }

                    rowIndex++;
                }

                page = ConstructPage(tableGrid, pageNumber);
            }

            return page;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function measures the heights of the page header, page footer and grid header and the first row in the grid
        /// in order to work out how manage pages might be required.
        /// </summary>
        private void MeasureElements()
        {
            double allocatedSpace = 0;

            //Measure the page header
            ContentControl pageHeader = new ContentControl();
            pageHeader.Content = CreateDocumentHeader();
            allocatedSpace = MeasureHeight(pageHeader);

            //Measure the page footer
            ContentControl pageFooter = new ContentControl();
            pageFooter.Content = CreateDocumentFooter(0);
            allocatedSpace += MeasureHeight(pageFooter);

            //Measure the table header
            ContentControl tableHeader = new ContentControl();
            tableHeader.Content = CreateTable(false);
            allocatedSpace += MeasureHeight(tableHeader);

            //Include any margins
            allocatedSpace += this.PageMargin.Bottom + this.PageMargin.Top;

            //Work out how much space we need to display the grid
            _availableHeight = this.PageSize.Height - allocatedSpace;

            //Calculate the height of the first row
            _avgRowHeight = MeasureHeight(CreateTempRow());

            //Calculate how many rows we can fit on each page
            double rowsPerPage = Math.Floor(_availableHeight / _avgRowHeight);

            if (!double.IsInfinity(rowsPerPage))
                _rowsPerPage = Convert.ToInt32(rowsPerPage);

            //Calculate the nuber of pages that we will need
            if (_rowCount > 0)
                _pageCount = Convert.ToInt32(Math.Ceiling(_rowCount / rowsPerPage));
        }

        /// <summary>
        /// This method constructs the document page (visual) to print
        /// </summary>
        private DocumentPage ConstructPage(Grid content, int pageNumber)
        {
            if (content == null)
                return null;

            //Build the page inc header and footer
            Grid pageGrid = new Grid();

            //Header row
            AddGridRow(pageGrid, GridLength.Auto);

            //Content row
            AddGridRow(pageGrid, new GridLength(1.0d, GridUnitType.Star));

            //Footer row
            AddGridRow(pageGrid, GridLength.Auto);

            ContentControl pageHeader = new ContentControl();
            pageHeader.Content = this.CreateDocumentHeader();
            pageGrid.Children.Add(pageHeader);

            if (content != null)
            {
                content.SetValue(Grid.RowProperty, 1);
                pageGrid.Children.Add(content);
            }

            ContentControl pageFooter = new ContentControl();
            pageFooter.Content = CreateDocumentFooter(pageNumber + 1);
            pageFooter.SetValue(Grid.RowProperty, 2);

            pageGrid.Children.Add(pageFooter);

            double width = this.PageSize.Width - (this.PageMargin.Left + this.PageMargin.Right);
            double height = this.PageSize.Height - (this.PageMargin.Top + this.PageMargin.Bottom);

            pageGrid.Measure(new Size(width, height));
            pageGrid.Arrange(new Rect(this.PageMargin.Left, this.PageMargin.Top, width, height));

            //return new DocumentPage(pageGrid);
            return new DocumentPage(pageGrid, PageSize, new Rect(content.DesiredSize), new Rect(content.DesiredSize));
        }



        private object CreateDocumentHeader()
        {
            Grid grid   = new Grid();
            grid.Margin = new Thickness(0, 0, 0, 10);

            var leftText                 = new TextBlock();
            leftText.Style               = this.HeaderLeftStyle;
            leftText.TextTrimming        = TextTrimming.CharacterEllipsis;
            leftText.Text                = this.HeaderLeftText;
            grid.Children.Add(leftText);

            var rightText                 = new TextBlock();
            rightText.Style               = this.HeaderRightStyle;
            rightText.TextTrimming        = TextTrimming.CharacterEllipsis;
            rightText.Text                = this.HeaderRightText;
            rightText.HorizontalAlignment = HorizontalAlignment.Right;
            rightText.SetValue(Grid.ColumnProperty, 1);
            grid.Children.Add(rightText);

            return grid;
        }



        private object CreateDocumentFooter(int pageNumber)
        {
            var dock = new DockPanel();
            dock.Margin = new Thickness(0, 10, 0, 0);

            var leftText    = new TextBlock();
            leftText.Style  = this.FooterLeftStyle;
            leftText.Text   = DateTime.Now.ToString("dd-MMM-yyy h:mm tt");
            DockPanel.SetDock(leftText, Dock.Left);
            dock.Children.Add(leftText);

            var rightText                 = new TextBlock();
            rightText.Style               = this.FooterRightStyle;
            rightText.Text                = "Page " + pageNumber.ToString() + " of " + this.PageCount.ToString();
            rightText.HorizontalAlignment = HorizontalAlignment.Right;
            DockPanel.SetDock(rightText, Dock.Right);
            dock.Children.Add(rightText);

            var centerText                 = new TextBlock();
            centerText.Style               = this.FooterCenterStyle;
            centerText.Text                = $"{_rowCount} items rendered"; 
            if (!AppTitle.IsBlank()) centerText.Text += $" by {AppTitle}";
            centerText.HorizontalAlignment = HorizontalAlignment.Center;
            dock.Children.Add(centerText);

            return dock;
        }

        /// <summary>
        /// Counts the number of rows in the document source
        /// </summary>
        /// <param name="itemsSource"></param>
        /// <returns></returns>
        private double CountRows(IEnumerable itemsSource)
        {
            int count = 0;

            if (itemsSource != null)
            {
                foreach (object item in itemsSource)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// The following function creates a temp table with a single row so that it can be measured and used to 
        /// calculate the totla number of req'd pages
        /// </summary>
        /// <returns></returns>
        private Grid CreateTempRow()
        {
            Grid tableRow = new Grid();

            if (_documentSource != null)
            {
                foreach (ColumnDefinition colDefinition in _tableColumnDefinitions)
                {
                    ColumnDefinition copy = XamlReader.Parse(XamlWriter.Save(colDefinition)) as ColumnDefinition;
                    tableRow.ColumnDefinitions.Add(copy);
                }

                foreach (object item in _documentSource.ItemsSource)
                {
                    int columnIndex = 0;
                    if (_documentSource.Columns != null)
                    {
                        foreach (DataGridColumn column in _documentSource.Columns)
                        {
                            if (column.Visibility == Visibility.Visible)
                            {
                                AddTableCell(tableRow, column, item, columnIndex, 0);
                                columnIndex++;
                            }
                        }
                    }

                    //We only want to measure teh first row
                    break;
                }
            }

            return tableRow;
        }

        /// <summary>
        /// This function counts the number of rows in the document
        /// </summary>
        private object CreateTable(bool createRowDefinitions)
        {
            if (_documentSource == null)
                return null;

            Grid table = new Grid();
            table.Style = this.GridContainerStyle;

            int columnIndex = 0;


            if (_documentSource.Columns != null)
            {
                double totalColumnWidth = _documentSource.Columns.Sum(column => column.Visibility == Visibility.Visible ? column.Width.Value : 0);

                foreach (DataGridColumn column in _documentSource.Columns)
                {
                    if (column.Visibility == Visibility.Visible)
                    {
                        AddTableColumn(table, totalColumnWidth, columnIndex, column);
                        columnIndex++;
                    }
                }
            }

            if (this.TableHeaderBorderStyle != null)
            {
                Border headerBackground = new Border();
                headerBackground.Style = this.TableHeaderBorderStyle;
                headerBackground.SetValue(Grid.ColumnSpanProperty, columnIndex);
                headerBackground.SetValue(Grid.ZIndexProperty, -1);

                table.Children.Add(headerBackground);
            }

            if (createRowDefinitions)
            {
                for (int i = 0; i <= _rowsPerPage; i++)
                    table.RowDefinitions.Add(new RowDefinition());
            }

            return table;

        }

        /// <summary>
        /// Measures the height of an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private double MeasureHeight(FrameworkElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.Measure(this.PageSize);
            return element.DesiredSize.Height;
        }

        /// <summary>
        /// Adds a column to a grid
        /// </summary>
        /// <param name="grid">Grid to add the column to</param>
        /// <param name="totalColumnWidth"></param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="column">Source column defintition which will be used to calculate the width of the column</param>
        private void AddTableColumn(Grid grid, double totalColumnWidth, int columnIndex, DataGridColumn column)
        {
            //double proportion = column.Width.Value / (this.PageSize.Width - (this.PageMargin.Left + this.PageMargin.Right));
            double proportion = column.ActualWidth / (this.PageSize.Width - (this.PageMargin.Left + this.PageMargin.Right));

            ColumnDefinition colDefinition = new ColumnDefinition();
            colDefinition.Width = new GridLength(proportion, GridUnitType.Star);

            grid.ColumnDefinitions.Add(colDefinition);

            TextBlock text = new TextBlock();
            text.Style = this.TableHeaderTextStyle;
            //text.TextTrimming = TextTrimming.CharacterEllipsis;
            //text.TextWrapping = TextWrapping.Wrap;
            text.Text = column.Header.ToString(); ;
            text.SetValue(Grid.ColumnProperty, columnIndex);

            grid.Children.Add(text);
            _tableColumnDefinitions.Add(colDefinition);
        }

        /// <summary>
        /// Adds a cell to a grid
        /// </summary>
        /// <param name="grid">Grid to add teh cell to</param>
        /// <param name="column">Source column definition which contains binding info</param>
        /// <param name="item">The binding source</param>
        /// <param name="columnIndex">Column index</param>
        /// <param name="rowIndex">Row index</param>
        private void AddTableCell(Grid grid, DataGridColumn column, object item, int columnIndex, int rowIndex)
        {
            if (column is DataGridTemplateColumn)
            {
                DataGridTemplateColumn templateColumn = column as DataGridTemplateColumn;
                ContentControl contentControl = new ContentControl();

                contentControl.Focusable = true;
                contentControl.ContentTemplate = templateColumn.CellTemplate;
                contentControl.Content = item;

                contentControl.SetValue(Grid.ColumnProperty, columnIndex);
                contentControl.SetValue(Grid.RowProperty, rowIndex);

                grid.Children.Add(contentControl);
            }
            else if (column is DataGridTextColumn)
            {
                DataGridTextColumn textColumn = column as DataGridTextColumn;
                TextBlock text = new TextBlock { Text = "Text" };

                text.Style = this.TableCellTextStyle;
                //text.TextTrimming = TextTrimming.CharacterEllipsis;
                text.DataContext = item;

                Binding binding = textColumn.Binding as Binding;

                //if (!string.IsNullOrEmpty(column.DisplayFormat))
                //binding.StringFormat = column.DisplayFormat;

                SetOtherProperties(text, textColumn);

                text.SetBinding(TextBlock.TextProperty, binding);

                text.SetValue(Grid.ColumnProperty, columnIndex);
                text.SetValue(Grid.RowProperty, rowIndex);

                grid.Children.Add(text);
            }
        }


        protected virtual void SetOtherProperties(TextBlock textBlk, DataGridTextColumn dgTextCol)
        {
            var styl = dgTextCol.ElementStyle;
            if (styl == null) return;

            var alignmnt = styl.FindSetter<TextAlignment>(TextBlock.TextAlignmentProperty);
            textBlk.TextAlignment = alignmnt ?? TextAlignment.Left;

            // can't wrap because we'll assume that all rows matches height of row1
            //var wrap = styl.FindSetter<TextWrapping>(TextBlock.TextWrappingProperty);
            //textBlk.TextWrapping = wrap ?? TextWrapping.NoWrap;
        }


        /// <summary>
        /// Adds a row to a grid
        /// </summary>
        private void AddGridRow(Grid grid, GridLength rowHeight)
        {
            if (grid == null)
                return;

            RowDefinition rowDef = new RowDefinition();

            if (rowHeight != null)
                rowDef.Height = rowHeight;

            grid.RowDefinitions.Add(rowDef);
        }

        #endregion

    }
}
