#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Code.Utils
{
    public class EnumGenerator : MonoBehaviour
    {
        [SerializeField] private string name;
        [SerializeField] private string[] entries;
        [SerializeField] private string path = "Assets/Code/Enums/";

        [SerializeField] private GurdenObjectEnum te4st; 
        
        [ContextMenu( "GenerateEnum" )]
        public void Go()
        {
            var filePathAndName = path + name + ".cs"; //The folder Scripts/Enums/ is expected to exist
            var enumString = $"/*\nDont touch\nGenerated from script\n*/\npublic enum {name}\n{{\n\t{string.Join(",\n\t", entries.Select(ConvertEnumEntry))}\n}}";
            
            File.WriteAllText(filePathAndName, enumString);
            /*
            using ( StreamWriter streamWriter = new StreamWriter( filePathAndName ) )
            {
                streamWriter.WriteLine( "public enum " + enumName );
                streamWriter.WriteLine( "{" );
                for( int i = 0; i < enumEntries.Length; i++ )
                {
                    streamWriter.WriteLine( "\t" + enumEntries[i] + "," );
                }
                streamWriter.WriteLine( "}" );
            }
            */
            AssetDatabase.Refresh();
        }

        private string ConvertEnumEntry(string raw)
        {
            var result = string.Empty;
            var makeWord = true;

            for (var i = 0; i < raw.Length; i++)
            {
                var symbol = raw[i];
                
                if (char.IsLetter(symbol))
                {
                    if (makeWord)
                    {
                        symbol = char.ToUpper(symbol);
                        makeWord = false;
                    }

                    result += symbol;
                }
                else
                {
                    makeWord = symbol == ' ';
                }
            }

            return result;
        }
    }
}

#endif