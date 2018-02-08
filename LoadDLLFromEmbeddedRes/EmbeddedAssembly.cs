using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoadDLLFromEmbeddedRes
{
    public static class EmbeddedAssembly
    {
        static Dictionary<string, Assembly> Index = new Dictionary<string, Assembly>();

        static EmbeddedAssembly()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                if (Index == null || Index.Count == 0) return null;
                if (Index.ContainsKey(e.Name)) return Index[e.Name];
                return null;
            };
        }

        static void Add(Assembly assembly) { Index.Add(assembly.FullName, assembly); }

        /// <summary>
        /// Load Assembly, DLL from Embedded Resources into memory.
        /// </summary>
        /// <param name="EmbeddedResource">Embedded Resource string. Example: WindowsFormsApplication1.SomeTools.dll</param>
        /// <param name="FileName">File Name. Example: SomeTools.dll</param>
        public static void Load(string EmbeddedResource, string FileName)
        {
            byte[] ByteArray = null;

            using (Stream ResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedResource))
            {
                // Either the file is not existed or it is not mark as embedded resource
                if (ResourceStream == null) throw new Exception(EmbeddedResource + " was not found in Embedded Resources.");

                // Get byte[] from the file from embedded resource
                ByteArray = new byte[(int)ResourceStream.Length];
                ResourceStream.Read(ByteArray, 0, (int)ResourceStream.Length);

                try
                {
                    // Add the assembly/dll into dictionary
                    Add(Assembly.Load(ByteArray));

                    return;
                }
                catch
                {
                    bool FileNotWritten = true;

                    // Define the temporary storage location of the DLL/assembly
                    string TempFilePath = Path.GetTempPath() + FileName;

                    using (SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider())
                    {
                        // Get the hash value from embedded DLL/assembly
                        string HashValue = BitConverter.ToString(SHA1.ComputeHash(ByteArray)).Replace("-", string.Empty);

                        // Determines whether the DLL/assembly is existed or not
                        if (File.Exists(TempFilePath))
                        {
                            // Get the hash value of the existed file
                            string HashValueOfExistingFile = BitConverter.ToString(
                                SHA1.ComputeHash(File.ReadAllBytes(TempFilePath))).Replace("-", string.Empty);

                            // Compare the existed DLL/assembly with the Embedded DLL/assembly
                            if (HashValue == HashValueOfExistingFile) FileNotWritten = false;
                        }
                    }

                    // Create the file on disk
                    if (FileNotWritten) File.WriteAllBytes(TempFilePath, ByteArray);

                    // Add the loaded DLL/assembly into dictionary
                    Add(Assembly.LoadFrom(TempFilePath));
                }
            }
        }
    }
}
