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
        public void printContent()
        {
            Console.WriteLine( "Barrel with: %d height: %d\nInside:", this.width, this.height);
            if( this.inside == null)
                Console.WriteLine("Empty\n");
            else
                this.inside.printContent();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var inList = new List<Barrel>();
            inList.Add( new Barrel(15, 31));
            inList.Add( new Barrel(4, 29));
            inList.Add( new Barrel(11, 22));

            List<Barrel> outList = getPackedBarrelsList(inList);
            foreach( Barrel elm in outList)
            {
                elm.printContent();
            }

        }

        List<Barrel> readBarrelsFile(string fileName)
        {
            return null;
        }

        static List<Barrel> getPackedBarrelsList(List<Barrel> inputList)
        {
            var outList = new List<Barrel>();
            //foreach( Barrel inElm in inputList)
            while(inputList.Count >0)
            {
                Barrel elm = inputList.
                if(outList.Count == 0)
                {
                    outList.Add( inElm );
                    inputList.Remove( inElm );
                    continue;
                }

                foreach( Barrel outElm in outList)
                {
                    if(inElm.fitsInsideOf( outElm ))
                    {
                        outElm.putElementInside(inElm);
                        inputList.Remove( inElm );
                        break;
                    }
                }
                // Defaults adding it to the outList directly
                outList.Add( inElm );
                inputList.Remove( inElm );
            }

            return outList;
        }
    }
}
