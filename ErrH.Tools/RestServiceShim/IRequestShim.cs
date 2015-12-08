using System.Collections.Generic;

namespace ErrH.Tools.RestServiceShim
{
    public interface IRequestShim
    {
        string UserName { get; set; }
        string Password { get; set; }

        RestMethod Method { get; set; }
        string Resource { get; set; }
        string CsrfToken { get; set; }
        object Body { get; set; }
        //FileShim    Attachment  { get; set; }

        Dictionary<string, string> Cookies { get; }
        Dictionary<string, object> Parameters { get; }
    }
}
