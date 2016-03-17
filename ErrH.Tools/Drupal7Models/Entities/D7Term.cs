namespace ErrH.Tools.Drupal7Models.Entities
{
    public class D7Term
    {
        public int     tid                     { get; set; }
        public int     vid                     { get; set; }
        public string  name                    { get; set; }
        public string  description             { get; set; }
        public string  format                  { get; set; }
        public int     weight                  { get; set; }
        public string  vocabulary_machine_name { get; set; }

        public override string ToString() => name;
    }
}
