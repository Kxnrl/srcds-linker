using System;
using System.Collections.Generic;
using System.IO;

namespace SRCDSLinker
{
    class Program
    {
        static List<string> dirToLink = new List<string>
        {
            "panorama",
            "resource",
        };

        static List<string> fileTypeToLink = new List<string>
        {
            "db",
            "vpk",
        };

        static void Main(string[] args)
        {
            Console.Clear();
            Console.Title = "SRCDS Linker v1.0 by Kyle";

            if (args.Length < 3)
            {
                Console.WriteLine("Usage: SRCDSLinker.exe {game} {sourceDir} {targetDir} {targetDir} ...");
                Environment.Exit(-1);
            }

            var xsrcds = args[0];
            var target = args[1];

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
                Console.WriteLine("Created target directory: {0}", target);
            }

            for (var index = 2; index < args.Length; index++)
            {
                if (!Directory.Exists(args[index]))
                {
                    Console.WriteLine("{0} does not exists.", args[index]);
                    continue;
                }

                var source = Path.Combine(args[index], xsrcds);

                if (!Directory.Exists(source))
                {
                    Console.WriteLine("{0} does not exists.", source);
                    continue;
                }

                // process dir
                foreach (var dir in dirToLink)
                {
                    var sp = Path.Combine(source, dir);
                    var tp = Path.Combine(target, dir);

                    if (!Directory.Exists(sp) && !Directory.Exists(tp))
                    {
                        Console.WriteLine("Skipped {0} because not found from source and target.");
                        continue;
                    }

                    if (!Directory.Exists(tp))
                    {
                        FileSystem.CopyDirectory(sp, tp);
                        Console.WriteLine("Moved directory {0} to {1}", sp, tp);
                    }

                    if (Directory.Exists(sp))
                    {
                        Directory.Delete(sp, true);
                    }

                    Win32.CreateSymbolicLink(sp, tp, Win32.SymbolicLink.Directory);
                    Console.WriteLine("Created directory link {0} to {1}", sp, tp);
                }

                // process file
                foreach (var ft in fileTypeToLink)
                {
                    foreach (var file in Directory.GetFiles(source, "*." + ft, SearchOption.TopDirectoryOnly))
                    {
                        var sp = Path.Combine(source, Path.GetFileName(file));
                        var tp = Path.Combine(target, Path.GetFileName(file));

                        if (!File.Exists(sp) && !File.Exists(tp))
                        {
                            Console.WriteLine("Skipped {0} because not found from source and target.");
                            continue;
                        }

                        if (!File.Exists(tp))
                        {
                            FileSystem.CopyFile(sp, tp);
                            Console.WriteLine("Moved file {0} to {1}", sp, tp);
                        }

                        if (File.Exists(sp))
                        {
                            File.Delete(sp);
                        }

                        Win32.CreateSymbolicLink(sp, tp, Win32.SymbolicLink.File);
                        Console.WriteLine("Created file link {0} to {1}", sp, tp);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Processed {0} to {1}", source, target);
            }

            Console.WriteLine("All done.");
            Console.ReadKey(true);
        }
    }
}
