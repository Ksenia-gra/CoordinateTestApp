using System.CodeDom;
using System.Printing;

namespace CoordinateTestApp.Data.Exceptions
{
	public class EmptyCacheException : Exception
	{
        public EmptyCacheException(string message) : base(message)
        {            
        }
    }
}
