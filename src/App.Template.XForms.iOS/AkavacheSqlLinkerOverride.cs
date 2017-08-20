using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akavache.Sqlite3;
using Foundation;
using UIKit;

namespace App.Template.XForms.iOS
{
    [Preserve]
    public static class LinkerPreserve
    {
        static LinkerPreserve()
        {
            throw new Exception(typeof(SQLitePersistentBlobCache).FullName);
        }
    }


    public class PreserveAttribute : Attribute
    {
    }
}