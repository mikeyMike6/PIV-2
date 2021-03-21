using System;
using System.Collections.Generic;

namespace PIV_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var cs = @"Data Source=HAL\MSSERVER;Initial Catalog=ZNorthwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            CRUD.Create(cs);
            CRUD.Read(cs);
            CRUD.Update(cs);
            CRUD.Delete(cs);
        }
    }
}