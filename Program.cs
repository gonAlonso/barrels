//#define ZDEBUG 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotNet
{
    public class Barrel
    {
        private int id;
        private int height;
        private int diameter;
        private Barrel inside = null;
        public Barrel(int ID, int diameter, int height)
        {
            this.id = ID;
            this.diameter = diameter;
            this.height = height;
        }

        public bool fitsInsideOf(Barrel outer)
        {
            if(outer.height > this.height && outer.diameter > this.diameter) return true;
            if(outer.diameter > this.height && outer.height > this.diameter) return true;
            return false;
        }

          public bool putInside(Barrel inner)
        {
            if( !inner.fitsInsideOf( this )) return false;
            if( this.inside != null ) return false;
            this.inside = inner;
            return true;
        }

        public int getVolume()
        {
            return this.diameter * this.height;
        }

        public void printContent()
        {
            Console.Write( $"{this.id} " );
            #if ZDEBUG
            Console.Write( $": {this.getArea()} " );
            #endif
            if( this.inside == null)
                Console.Write("\n");
            else
                this.inside.printContent();
        }
    }


    public class ByVolume : IComparer<Barrel>
    {
        public int Compare(Barrel A, Barrel B)
        {
            return B.getVolume() - A.getVolume();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var inputList = readBarrelsFile( "barrels.txt" );
            inputList.Sort( new ByVolume() );
            var packedList = packBarrelsOfList(inputList);
            printBarrelsList(packedList);
        }

        static void printBarrelsList(List<Barrel> list)
        {
            Console.WriteLine( "Barrels list" );
            foreach( Barrel elm in list)
            {
                elm.printContent();
            }

        }


        static List<Barrel> readBarrelsFile(string fileName)
        {
            List<Barrel> barrelList = new List<Barrel>();
            try
            {
                using (var inputFile = new StreamReader( fileName ))
                {
                    while( !inputFile.EndOfStream)
                    {
                        string newLine = inputFile.ReadLine();
                        string[] newBarrelData = newLine.Split(' ');
                        int id = int.Parse( newBarrelData[0] );
                        int diameter = int.Parse( newBarrelData[1] );
                        int height = int.Parse( newBarrelData[2] );
                        barrelList.Add( new Barrel( id, diameter, height));
                    }

                    return barrelList;
                }
            }
            catch(FormatException f)
            {
                Console.WriteLine($"Unexpected file data: {f}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be read: {e}");
            }
            return null;
        }


        static List<Barrel> packBarrelsOfList(List<Barrel> inputList)
        {
            if( inputList.Count == 0 ) return null;

            var outList = new List<Barrel>();

            while( inputList.Count > 0 )
            {
                var outBarrel = inputList.First();
                outList.Add( outBarrel );
                inputList.Remove( outBarrel );

                var it = inputList.GetEnumerator();
                while( it.MoveNext() )
                {
                    var inputBarrel = it.Current;
                    if( outBarrel.putInside(inputBarrel))
                    {
                        outBarrel = inputBarrel;
                        inputList.Remove( inputBarrel );
                        it = inputList.GetEnumerator();
                    }
                }
            }
            return outList;
        }

    }
}
