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
        public bool fitsInsideOf(Barrel outer)
        {
            if(outer.height > this.height && outer.width > this.width) return true;
            if(outer.width > this.height && outer.height > this.width) return true;
            return false;
        }
        public bool putElementInside(Barrel inner)
        {
            if (!inner.fitsInsideOf(this)) return false;
            this.inside = inner;
            return true;
        }

        public Barrel getInnerMostBarrel()
        {
            if(this.inside==null)
                return this;
            else
                return this.inside.getInnerMostBarrel();
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
            Queue<Barrel> packedList = getPackedBarrelsList(inList);
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


        static Queue<Barrel> getPackedBarrelsList(Queue<Barrel> inputList)
        {
            var outList = new Queue<Barrel>();
            while(inputList.Count >0)
            {
                Barrel inElm = inputList.Dequeue();
                if(outList.Count == 0)
                {
                    outList.Enqueue( inElm );
                    continue;
                }

                foreach( Barrel outElm in outList)
                {
                    Barrel innerBarrel = outElm.getInnerMostBarrel();
                    if( inElm.fitsInsideOf( innerBarrel ))
                    {
                        innerBarrel.putElementInside(inElm);
                        inElm = null;
                        break;
                    }
                }

                if ( inElm!=null ) 
                {
                    outList.Enqueue( inElm );
                }
            }
            return outList;
        }
    }
}
