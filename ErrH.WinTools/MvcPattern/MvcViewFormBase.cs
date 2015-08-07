using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.MvcPattern;
using ErrH.WinTools.CollectionShims;

namespace ErrH.WinTools.MvcPattern
{
    public class MvcViewFormBase : Form
    {
        protected MvcControllerBase _controller;


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (Site != null && Site.DesignMode) return;

            _controller = Step1_SetupMVC();

            Step2_HandleModelEvents();
            Step3_HandleControllerEvents();
            Step4_ToggleWidgets();
            Step5_ForwardWidgetEvents();
            Step6_SortLists();
            Step7_ApplyCosmetics1();
            Step8_ApplyCosmetics2();

            _controller.ToggleElements(CtrlState.Disabled);
            _controller.OnViewLoad();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (Site != null && Site.DesignMode) return;
            if (_controller == null) return;

            _controller.OnViewUnload();
            _controller = null;
        }



        protected virtual MvcControllerBase Step1_SetupMVC()
        {
            throw Error.Undone("SetupMVC",
                GetType().Name + " must override SetupMVC().");
        }

        protected virtual void Step2_HandleModelEvents() { }
        protected virtual void Step3_HandleControllerEvents() { }
        protected virtual void Step4_ToggleWidgets() { }
        protected virtual void Step5_ForwardWidgetEvents() { }
        protected virtual void Step6_SortLists() { }
        protected virtual void Step7_ApplyCosmetics1() { }
        protected virtual void Step8_ApplyCosmetics2() { }



        public SortableBindingList<T> BoundList<T>(List<T> list) where T : class, INotifyPropertyChanged
        {
            if (list == null) return null;
            //return new BindingList<T>(list);
            return new SortableBindingList<T>(list);
        }

    }
}
