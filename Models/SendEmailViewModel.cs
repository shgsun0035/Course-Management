using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment.Models
{
    public class SendEmailViewModel
    {
        [Required(ErrorMessage = "Please enter an email address.")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter the contents")]
        [AllowHtml]
        public string Contents { get; set; }

    }
}