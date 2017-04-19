using System;
using System.Reflection;

namespace Testy.Helper
{
    public static class GetPath
    {
        public static String testDllPath = Assembly.GetExecutingAssembly().Location;
    }
}
