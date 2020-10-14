#define ZDEBUG 
using System;
using System.Collections.Generic;
using System.IO;

namespace dotNet
{
    internal class Barrel
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
            if (!this.inside.fitsInsideOf(inner)) return false;
            
            Barrel temp = this.inside;
            this.inside = inner;
            inner.inside = temp;
            return true;
        }

        public void printContent()
        {
            Console.Write( $"{this.id} " );
            if( this.inside == null)
                Console.Write("\n");
            else
                this.inside.printContent();
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            var inList = readBarrelsFile( "barrels.txt" );
            Queue<Barrel> packedList = createPackedBarrelsList(inList);
            foreach( Barrel elm in packedList)
            {
                elm.printContent();
            }

        }

        static Queue<Barrel> readBarrelsFile(string fileName)
        {
            Queue<Barrel> barrelList = new Queue<Barrel>();
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
                        barrelList.Enqueue( new Barrel( id, diameter, height));
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


        static Queue<Barrel> createPackedBarrelsList(Queue<Barrel> inputList)
        {
            var outList = new Queue<Barrel>();
            while( inputList.Count >0)
            {
                Barrel inElm = inputList.Dequeue();
                if( outList.Count == 0)
                {
                    #if ZDEBUG
                    Console.WriteLine( $"First elm is {inElm.ID}");
                    #endif
                    outList.Enqueue( inElm );
                    continue;
                }

                if( outList.Peek().fitsInsideOf( inElm ) )
                {
                    Barrel takenFromOutList = outList.Dequeue();
                    inElm.putElementInside( takenFromOutList );
                    outList.Enqueue( inElm );
                    #if ZDEBUG
                    Console.WriteLine( $"{takenFromOutList.ID} goes inside {inElm.ID}, as root");
                    #endif
                    continue;
                }

                foreach( Barrel outElm in outList)
                {
                    if (outElm.putElementInside( inElm ))
                    {
                        #if ZDEBUG
                        Console.WriteLine( $"{inElm.ID} goes inside {outElm.ID}, as child");
                        #endif
                        inElm = null;
                        break;
                    }
                }

                if ( inElm!=null ) 
                {
                    #if ZDEBUG
                    Console.WriteLine( $"{inElm.ID} goes to the out List root ELM");
                    #endif
                    outList.Enqueue( inElm );
                }
            }
            return outList;
        }
    }
}
