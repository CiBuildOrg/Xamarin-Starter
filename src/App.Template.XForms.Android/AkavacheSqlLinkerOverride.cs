using System;
using Akavache.Sqlite3;

namespace App.Template.XForms.Android
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