using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Models
{
    public class Authenticate
    {
        [Required]
        public string Username { get; set; }

        private string _password;

        [Required]
        public string Password
        {
            get
            {
                return this._password;
            }

            set
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(this._password));
                    var sb = new StringBuilder(hash.Length * 2);

                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("X2"));
                    }

                    this._password = sb.ToString();
                }
            }
        }
    }
}
