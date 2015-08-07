using ErrH.Tools.Extensions;

namespace ErrH.Tools.Drupal7Models.Entities
{
    public static class ID7NodeExtensions
    {

        public static bool IsValidNode(this ID7Node node)
        {
            string m;
            return node.IsValidNode(out m);
        }


        public static bool IsValidNode(this ID7Node node, out string validationMsg)
        {
            if (node == null)
            {
                validationMsg = "Node is NULL.";
                return false;
            }
            if (node.nid < 1)
            {
                validationMsg = "Invalid node [nid: {0}].".f(node.nid);
                return false;
            }

            validationMsg = "Node is valid.";
            return true;
        }

    }
}
