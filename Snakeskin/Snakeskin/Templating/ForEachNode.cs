using System.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class ForEachNode : ParentNode
    {
        private readonly string _iterator;
        private readonly string _array;
        
        public ForEachNode(string iterator, string array)
        {
            _iterator = iterator;
            _array = array;
        }
        
        public override StringBuilder Render(StringBuilder builder)
        {
            //todo: this
            throw new System.NotImplementedException();
        }
    }
}