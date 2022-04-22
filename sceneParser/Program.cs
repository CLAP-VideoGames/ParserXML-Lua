using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

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
                lua.Write("\"" + entidades[i] + "\" ,");
            lua.WriteLine("\"" + entidades[entidades.Count - 1] + "\" }");
            lua.Write("\n");
            if (args.Length == 3){
                StreamReader codeLua = new StreamReader(args[2]);
                string code;
                while ((code = codeLua.ReadLine())  != null)
                    lua.WriteLine(code);
            }
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
                lua.Write(numberIndentations + xtr.Name + " = \"");

                if (xtr.Name == "rotation"){
                    Quaternion q;

                    NumberFormatInfo provider = new NumberFormatInfo();
                    provider.NumberDecimalSeparator = ".";

                    xtr.MoveToAttribute(0);
                    q.W = (float) Convert.ToDouble(xtr.Value, provider);
                    xtr.MoveToAttribute(1);
                    q.X = (float) Convert.ToDouble(xtr.Value, provider);
                    xtr.MoveToAttribute(2);
                    q.Y = (float) Convert.ToDouble(xtr.Value, provider);
                    xtr.MoveToAttribute(3);
                    q.Z = (float) Convert.ToDouble(xtr.Value, provider);
                    
                    Vector3 rotation = ToEulerAngles(q);
                    rotation = toRadians(rotation);


                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    lua.Write(" " + rotation.X.ToString(nfi) + " ," + rotation.Y.ToString(nfi) + " ," + rotation.Z.ToString(nfi) +  " ");
                    lua.WriteLine("\",");

                    continue;
                }

                xtr.MoveToAttribute(0);
                lua.Write(" " + xtr.Value + " ");
                for (int i = 1; i < xtr.AttributeCount; i++){
                    xtr.MoveToAttribute(i);
                    lua.Write("," + xtr.Value + " ");
                }
                lua.WriteLine("\",");
            }

            nSpaces -= 4;
            numberIndentations = new string(c, nSpaces);

            lua.WriteLine(numberIndentations + "},");
        }

        static void processComponents(ref StreamWriter lua, ref XmlTextReader xtr, ref string numberIndentations, ref int nSpaces, char c) {
            do readNextElement(ref xtr);
            while (xtr.GetAttribute("type") == "IDPropertyGroup");

            while(xtr.NodeType != XmlNodeType.EndElement){
                if(xtr.GetAttribute("name") == "Enabled"){

                    lua.Write(numberIndentations + xtr.GetAttribute("name") + " = ");
                    string[] data_ = xtr.GetAttribute("data").Split(',');

                    for (int i = 0; i < data_.Length; i++)
                        lua.WriteLine(data_[i] + ",");

                    readNextElement(ref xtr);
                    continue;
                }
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

        static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles = new Vector3();

            // roll / x
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        static Vector3 toRadians(Vector3 q){
            q.X = (float) ((180 / Math.PI) * q.X);
            q.Y = (float) ((180 / Math.PI) * q.Y);
            q.Z = (float) ((180 / Math.PI) * q.Z);

            return q;
        }
    }
}
