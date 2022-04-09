using System;
using System.Xml;
using System.IO;

namespace sceneParser {
    class Program{

        static void Main(string[] args) {
            StreamWriter lua = new StreamWriter(args[0]);
            XmlTextReader xtr = new XmlTextReader(args[1]);
            lua.WriteLine("mapEntities = {");
            char c = ' ';
            int nSpaces = 4;
            string numberIndentations = new string (c, nSpaces);

            while (xtr.Read()){
                if (xtr.HasAttributes) {
                    if (xtr.Name == "xml") continue;
                    else if (xtr.Name == "node"){
                        xtr.MoveToNextAttribute();
                        //Name
                        lua.WriteLine(numberIndentations + xtr.Value + " = {");
                        processTransform(ref lua, ref xtr, ref numberIndentations, ref nSpaces, c);

                        //xtr.ReadEndElement();
                        //int l = xtr.AttributeCount;


                        break;
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
            }
            lua.WriteLine("}");
            xtr.Close();
            lua.Close();
        }

        static void processTransform(ref StreamWriter lua, ref XmlTextReader xtr, ref string numberIndentations, ref int nSpaces, char c){
            nSpaces += 4;
            numberIndentations = new string(c, nSpaces);
            lua.WriteLine(numberIndentations + "Transform = {");
            nSpaces += 4;
            numberIndentations = new string(c, nSpaces);
            int numberTransform = 3;

            for (int j = 0; j < numberTransform; j++){
                xtr.Read();
                xtr.Read();
                lua.Write(numberIndentations + xtr.Name + " = { ");

                xtr.MoveToAttribute(0);
                lua.Write(" " + xtr.Value + " ");
                for (int i = 1; i < xtr.AttributeCount; i++)
                {
                    xtr.MoveToAttribute(i);
                    lua.Write("," + xtr.Value + " ");
                }
                lua.WriteLine("},");
            }

            nSpaces -= 5;
            numberIndentations = new string(c, nSpaces);

            lua.WriteLine(numberIndentations + " },");
        }
    }
}
