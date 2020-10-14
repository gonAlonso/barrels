#define ZDEBUG 
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
        private int width;
        private Barrel inside = null;
        public Barrel(int ID, int width, int height)
        {
            this.id = ID;
            this.width = width;
            this.height = height;
        }

        public int ID{ get {return this.id;} }
        public bool fitsInsideOf(Barrel outer)
        {
            if(outer.height > this.height && outer.width > this.width) return true;
            if(outer.width > this.height && outer.height > this.width) return true;
            return false;
        }
        public bool putElementInside(Barrel inner)
        {
            if (!inner.fitsInsideOf(this)) return false;
            if(this.inside == null)
            {
                this.inside = inner;
                return true;
            }
            if ( !this.inside.fitsInsideOf( inner )) return false;
            
            Barrel temp = this.inside;
            this.inside = inner;
            inner.inside = temp;
            return true;
        }

          public bool putInside(Barrel inner)
        {
            if( !inner.fitsInsideOf( this )) return false;
            if( this.inside != null ) return false;
            this.inside = inner;
            return true;
        }

        public int getArea()
        {
            return this.width * this.height;
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


    public class ByArea : IComparer<Barrel>
    {
        public int Compare(Barrel A, Barrel B)
        {
            return B.getArea() - A.getArea();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var inputList = readBarrelsFile( "barrels.txt" );
            printBarrelsList(inputList);
            inputList.Sort( new ByArea() );
            printBarrelsList(inputList);
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
