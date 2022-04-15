using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace sceneParser {
    class Program{

        static void Main(string[] args) {
            StreamWriter lua = new StreamWriter(args[0]);
            XmlTextReader xtr = new XmlTextReader(args[1]);
            lua.WriteLine("scene = {");
            char c = ' ';
            int nSpaces = 4;
            string numberIndentations = new string (c, nSpaces);

            List<string> entidades = new List<string>();

            while (xtr.Read()){
                if (xtr.NodeType == XmlNodeType.Element){
                    if (xtr.HasAttributes) {
                        if (xtr.Name == "xml") continue;
                        else if (xtr.Name == "node") {
                            processEntity(ref lua, ref xtr, ref numberIndentations, ref nSpaces, c, ref entidades);
                        }
                    }
                }
            }
            lua.WriteLine("}");

            lua.WriteLine("");
            lua.Write("entities = {");
            for (int i = 0; i < entidades.Count - 1; i++)
                lua.Write(entidades[i] + " ,");
            lua.WriteLine(entidades[entidades.Count - 1] + " }");
            
            xtr.Close();
            lua.Close();
        }

        static void processEntity(ref StreamWriter lua, ref XmlTextReader xtr, ref string numberIndentations, ref int nSpaces, char c, ref List<string> ent){
            xtr.MoveToNextAttribute();
            //Name
            ent.Add(xtr.Value);
            lua.WriteLine(numberIndentations + xtr.Value + " = {");
            nSpaces += 4;
            numberIndentations = new string(c, nSpaces);
            processTransform(ref lua, ref xtr, ref numberIndentations, ref nSpaces, c);
            readNextElement(ref xtr);
            processComponents(ref lua, ref xtr, ref numberIndentations, ref nSpaces, c);
            readNextElement(ref xtr);
            processMeshData(ref lua, ref xtr, ref numberIndentations, ref nSpaces, c);
            nSpaces -= 4;
            numberIndentations = new string(c, nSpaces);
            lua.WriteLine(numberIndentations + "},");
        }

        static void processTransform(ref StreamWriter lua, ref XmlTextReader xtr, ref string numberIndentations, ref int nSpaces, char c){
            lua.WriteLine(numberIndentations + "Transform = {");
            nSpaces += 4;
            numberIndentations = new string(c, nSpaces);
            int numberTransform = 3;

            for (int j = 0; j < numberTransform; j++){
                readNextElement(ref xtr);
                lua.Write(numberIndentations + xtr.Name + " = { ");

                xtr.MoveToAttribute(0);
                lua.Write(" " + xtr.Value + " ");
                for (int i = 1; i < xtr.AttributeCount; i++){
                    xtr.MoveToAttribute(i);
                    lua.Write("," + xtr.Value + " ");
                }
                lua.WriteLine("},");
            }

            nSpaces -= 4;
            numberIndentations = new string(c, nSpaces);

            lua.WriteLine(numberIndentations + "},");
        }

        static void processComponents(ref StreamWriter lua, ref XmlTextReader xtr, ref string numberIndentations, ref int nSpaces, char c) {
            do readNextElement(ref xtr);
            while (xtr.GetAttribute("type") == "IDPropertyGroup");

            while(xtr.NodeType != XmlNodeType.EndElement){
                lua.WriteLine(numberIndentations + xtr.GetAttribute("name") + " = {");
                nSpaces += 4;
                numberIndentations = new string(c, nSpaces);

                string[] data = xtr.GetAttribute("data").Split(',');
                for (int i = 0; i < data.Length - 1; i++)
                    lua.WriteLine(numberIndentations + data[i] + ",");
                
                lua.WriteLine(numberIndentations + data[data.Length - 1]);
                nSpaces -= 4;
                numberIndentations = new string(c, nSpaces);
                lua.WriteLine(numberIndentations + "},");
                readNextElement(ref xtr);
            }
        }
    
        static void readNextElement(ref XmlTextReader xtr) {
            do{
                xtr.Read();
            } while (xtr.NodeType != XmlNodeType.Element && xtr.NodeType != XmlNodeType.EndElement);
        }

        static void processMeshData(ref StreamWriter lua, ref XmlTextReader xtr, ref string numberIndentations, ref int nSpaces, char c){
            lua.WriteLine(numberIndentations + "MeshRenderer = {");
            nSpaces += 4;
            numberIndentations = new string(c, nSpaces);
            lua.WriteLine(numberIndentations + "MeshFile = \"" + xtr.GetAttribute("meshFile") + "\",");
            nSpaces -= 4;
            numberIndentations = new string(c, nSpaces);
            lua.WriteLine(numberIndentations + "}");
        }
    }
}
