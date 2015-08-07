﻿using ErrH.Tools.Drupal7Models.Entities;

namespace ErrH.Tools.Drupal7Models.DTOs
{
    public class D7UserSession
    {
        public string sessid { get; set; }
        public string session_name { get; set; }
        public string token { get; set; }

        public D7User user { get; set; }

    }
}
