#region @replace #TestObj.StringLiteral:"replaced string literal", Namespace

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
        #region @if #Property.?TestBoolProp
        // This one had TestBoolProp true
        #endregion
        public Placeholder Name { get; set; }
        #endregion
        #endregion
    }
    #endregion
    
    #region @if ?TestBool
    // Optional comment
    #endregion
}
#endregion
