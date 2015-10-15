namespace ErrH.Tools.Drupal7Models.Entities
{
    public class D7NodeBase : ID7Node
    {
        public int nid { get; set; }


        //public int? vid { get; set; }
        public int uid { get; set; }

        //  entity properties that doesn't support writing
        //
        //public bool is_new { get; set; }
        //public string url { get; set; }
        //public string edit_url { get; set; }
        //public long changed { get; set; }
        //public int comment_count { get; set; }
        //public int comment_count_new { get; set; }
        //public List<int> comments { get; set; }

        //public string language { get; set; }
        //public int status { get; set; }
        //public int promote { get; set; }
        //public int sticky { get; set; }
        //public long created { get; set; }
        //public string log { get; set; }
        //public string revision { get; set; }
        //public int comment { get; set; }
        //public List<int> body { get; set; }

        //public CleanResourceId author { get; set; }


        public virtual string title { get; set; }
        public string type { get; set; }
        //public string  uri    { get; set; }

        public D7NodeBase()
        {
            //this.language = "und";
            //this.author = CleanResourceId.User(1);
        }
    }
}
