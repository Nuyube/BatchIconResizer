using System;
using System.Drawing;
using System.IO;

namespace BatchImageResizer {

    public static class BatchImageResizer {

        private static void Main( string[ ] args ) {
            //Fail if we don't have any arguments
            if (args.Length is 0) {
                Console.WriteLine( "USAGE: BatchImageResizer (file)" );
                return;
            }
            //Iterate over the arguments list
            for (int i = 0; i < args.Length; i++) {
                //Get the file path from the arguments
                string path = args[ i ];
                if (!File.Exists( path )) {
                    Console.WriteLine( "ERROR: The file could not be found." );
                    return;
                }
                //Calculate the file full path - extension
                string pathsub = path.Substring( 0, path.LastIndexOf( '.' ) );
                try {
                    //Read our bitmap into a file
                    using Bitmap b = new( path );
                    //Warn if our input isn't square.
                    if (b.Width != b.Height) {
                        Console.WriteLine( "WARNING: BatchImageResizer expects 1:1 files of a power of two size." );
                    }
                    //Our exponent to divide by. This way, we write the high quality image into a low quality container.
                    int Factor = 1;
                    //Main loop.
                    while (true) {
                        //Calculate the new size of sides
                        int newS = (int) (b.Width / Math.Pow( 2, Factor ));
                        //If it's less than 32, exit.
                        if (newS < 32) break;
                        else {
                            //Else, create our new bitmap
                            Bitmap B = new( newS, newS );
                            //Create a Graphics from it
                            Graphics g = Graphics.FromImage( B );
                            //Draw the image rescaled
                            g.DrawImage( b, 0, 0, newS, newS );
                            //Flush the graphics, waiting for them to finish
                            g.Flush( System.Drawing.Drawing2D.FlushIntention.Sync );
                            //Save the image
                            string newFileName = pathsub + newS.ToString( ) + ".png";
                            B.Save( newFileName );
                            //Dispose bitmap and graphics.
                            g.Dispose( );
                            B.Dispose( );
                            //Write a message to the console saying that we've completed our task.
                            Console.WriteLine( "Wrote " + newFileName );
                            //Increment our factor.
                            Factor++;
                        }
                    }
                }
                //Fail if we can't find the file.
                catch (FileNotFoundException) {
                    Console.WriteLine( $"ERROR: The file could not be found: {path}." );
                    continue;
                }
            }
        }
    }
}