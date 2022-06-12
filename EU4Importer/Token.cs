using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal record Token
    {
        public string content;
        public string value;

    }
    internal record ValueToken : Token
    {
        public ValueToken() { }
        public ValueToken(string content)
        {
            this.content = content;
        }
        public ValueToken(string content, string value)
        {
            this.content = content;
            this.value = value;
        }
    }
    internal record ContentToken : Token
    {
        public ContentToken() { }
        public ContentToken(string content)
        {
            this.content = content;
        }
    }
    internal record OpenBracketsToken : Token
    {

    }
    internal record CloseBracketsToken : Token
    {

    }
    internal record EqualsToken : Token
    {

    }
    internal record CharToken : Token
    {
        public CharToken(char content)
        {
            this.content = content.ToString();
        }

    }
    internal record EmptyToken : Token
    {

    }

}
