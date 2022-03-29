using System;
using System.Xml;
using System.IO;

namespace sceneParser {
    class Program{
        static void Main(string[] args) {
            string writeFile = args[0];
            int n = args.Length;
            StreamWriter lua = new StreamWriter(writeFile);
            string readFile = args[1];
            XmlTextReader xtr = new XmlTextReader(readFile);
            lua.WriteLine("map = {");
            char c = ' ';
            string numberIndentations = new string (c, 4);
            while (xtr.Read()){

                if (xtr.HasAttributes) {
                    lua.WriteLine(numberIndentations + xtr.Name + " Attribute");
                    for (int i = 0; i < xtr.AttributeCount; i++)
                    {
                        xtr.MoveToAttribute(i);
                        lua.WriteLine(numberIndentations+ "Nam: " + xtr.Name + ", Value: " + xtr.Value);
                    }
                    xtr.MoveToElement();
                }
                //if (xtr.NodeType== XmlNodeType.Element && xtr.Name == "Name"){
                //    Console.WriteLine("he");
                //}
                //switch (reader.NodeType)
                //{
                //    case XmlNodeType.Element: // The node is an element.
                //        Console.Write("<" + reader.Name);
                //        Console.WriteLine(">");
                //        break;
                //    case XmlNodeType.Text: //Display the text in each element.
                //        Console.WriteLine(reader.Value);
                //        break;
                //    case XmlNodeType.EndElement: //Display the end of the element.
                //        Console.Write("</" + reader.Name);
                //        Console.WriteLine(">");
                //        break;
                //}
            }

            lua.WriteLine("}");
            xtr.Close();
            lua.Close();
        }
    }
}
