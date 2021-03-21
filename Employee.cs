using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PIV_2
{
    class Employee
    {
        public int IDpracownika { get; set; }
        public string Imię { get; set; }
        public string Nazwisko { get; set; }
        public string Stanowisko { get; set; }

        public override string ToString()
        {
            return $"{IDpracownika}\t{Imię} {Nazwisko}\t{Stanowisko}";
        }
    }
}