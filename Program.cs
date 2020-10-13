using System;
using System.Collections.Generic;

namespace dotNet
{
    internal class Barrel
    {
        private int height;
        private int width;
        private Barrel inside = null;
        public Barrel(int width, int height)
        {
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
            Console.Write( $"Barrel width: {this.width} height: {this.height} >> ");
            if( this.inside == null)
                Console.Write("Empty\n");
            else
                this.inside.printContent();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating Barrels!");
            var inList = new Queue<Barrel>();
            inList.Enqueue( new Barrel(15, 31));
            inList.Enqueue( new Barrel(4, 29));
            inList.Enqueue( new Barrel(11, 22));

            Queue<Barrel> outList = getPackedBarrelsList(inList);
            foreach( Barrel elm in outList)
            {
                elm.printContent();
            }

        }

        List<Barrel> readBarrelsFile(string fileName)
        {
            return null;
        }

        static Queue<Barrel> getPackedBarrelsList(Queue<Barrel> inputList)
        {
            var outList = new Queue<Barrel>();
            //foreach( Barrel inElm in inputList)
            while(inputList.Count >0)
            {
                Barrel inElm = inputList.Dequeue();
                if(outList.Count == 0)
                {
                    Console.Write( "First element: ");
                    inElm.printContent();
                    outList.Enqueue( inElm );
                    continue;
                }

                foreach( Barrel outElm in outList)
                {
                    Barrel innerBarrel = outElm.getInnerMostBarrel();
                    Console.Write( "Testing: ");
                    inElm.printContent();
                    if( inElm.fitsInsideOf( innerBarrel ))
                    {
                        Console.WriteLine( "Fits!!");
                        innerBarrel.putElementInside(inElm);
                        inElm = null;
                        break;
                    }
                }

                if ( inElm!=null ) 
                {
                    inElm.printContent();
                    outList.Enqueue( inElm );
                }
            }
            Console.Write( "\n");
            return outList;
        }
    }
}
