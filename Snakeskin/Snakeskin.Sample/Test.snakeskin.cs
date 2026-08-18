#region @replace #TestObj.StringLiteral:"replaced string literal", Namespace

using System;

namespace Namespace
{
    #region @replace ClassName
    internal class ClassName
    {
        #region @remove
        public class Placeholder
        {
        }
        #endregion
        public ClassName()
        {
            var text = string.Empty;
            
            #region @foreach String:"replaced by string array literal" in Strings[]
            text += "replaced by string array literal";
            #endregion
            
            var stringLiteral = "replaced string literal";
        }

        #region @foreach #Property in #Properties[]

        #region @replace #Property.Type:"Placeholder", #Property.Name
        public Placeholder Name { get; set; }
        #endregion
        #endregion
    }
    #endregion
}
#endregion
             