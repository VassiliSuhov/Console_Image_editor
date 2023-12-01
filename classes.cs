namespace Editor
{

    internal class Vector
    {
        public double x { get; set; }
        public double y { get; set; }
        public Vector(float x_ , float y_) {
            x = x_;
            y = y_;
        }
        public void RandomVector(Random generator )
        {
            int[] multplayer = { -1, 1 };
            x = generator.NextDouble() * 3 * multplayer[generator.Next(0, 2)];
            y = generator.NextDouble() * 3 * multplayer[generator.Next(0, 2)];
        }
        public void PrintVector() 
        {
            Console.WriteLine($"x : {this.x} || y : {this.y}");
        }
    }



    internal class PerlinNoiseMap
    {
        public int[,] noise_map { get; set; }


        // gets a coord and the dimensions of a map , check if the coords are inside that map
        public static bool isValidCoord(int[] coord, int[] current_map_size)
        {
            return (coord[0] >= 0 && coord[1] >= 0 && coord[0] < current_map_size[0] && coord[1] < current_map_size[1]);
        }






        //
        public static int[,] produce_offset_ranges(int distance)
        {
            int[,] offsets = new int[(distance * 2 + 1) * (distance * 2 + 1), 2];

            int cpt = 0;
            for (int x = -distance; x <= distance; x++)
            {
                for (int y = -distance; y <= distance; y++)
                {
                    offsets[cpt, 0] = x;
                    offsets[cpt, 1] = y;
                    cpt++;

                }
            }
            return offsets;
        }





        public static float coord_distance(float[] coord1, int[] coord2)
        {
            return (float)Math.Sqrt(Math.Pow(coord1[0] - (float)coord2[0], 2) + Math.Pow(coord1[1] - (float)coord2[1], 2));
        }

        // given two maps of different size ,
        public static List<int[]> GetSurroundingCoords(float[] orignal_coord, int[] rounded_coords, int[] target_map_size , int area = 1)
        {

            const float SQRT_2 = 1.41421356237f;

            List<int[]> neighbors = new List<int[]>();

            int[,] NEIGHBOR_OFFSETS = produce_offset_ranges(area);

            for (int i = 0; i < NEIGHBOR_OFFSETS.GetLength(0); i++)
            {
                int[] new_cord = { rounded_coords[0] + NEIGHBOR_OFFSETS[i, 0], rounded_coords[1] + NEIGHBOR_OFFSETS[i, 1] };
                if (isValidCoord(new_cord, target_map_size))
                {
                    if (coord_distance(orignal_coord, new_cord) <= SQRT_2)
                    {

                        neighbors.Add(new_cord);
                    }
                }
            }
            return neighbors;
        }
        public static int PointDensity(Vector[,] v_map, int pos_x, int pos_y, int[] grad_map_size)
        {

            
            int[] v_bounds = { v_map.GetLength(0), v_map.GetLength(1) };
            float[] aproximated_point = { pos_x * v_bounds[0] / grad_map_size[0], pos_y * v_bounds[1] / grad_map_size[1] };
            int[] real_points = { (int)Math.Round(aproximated_point[0]), (int)Math.Round(aproximated_point[1]) };
            List<int[]> v_indexs = GetSurroundingCoords(aproximated_point, real_points, v_bounds , 1);


            double Dot_sum = 0;
            int cpt = 0;
            while (cpt < v_indexs.Count)
            {
                Vector v = v_map[v_indexs[cpt][0], v_indexs[cpt][1]];
                Vector offset = new Vector(aproximated_point[0] - v_indexs[cpt][0] , aproximated_point[1] - v_indexs[cpt][1] );
                Dot_sum += DotProduct(v, offset);

                offset = null;
                cpt++;
            }
            v_indexs = null;
            aproximated_point = null;
            v_bounds = null;
            return (int)(Dot_sum * 50);

        }
        public static void Create_density_map(Vector[,] v_map, int[,] d_map)
        {
            int[] map_size = { d_map.GetLength(0), d_map.GetLength(1) };
            for (int i = 0; i < map_size[0]; i++)
            {
                for (int j = 0; j < map_size[1] - 1; j++)
                {
                    d_map[i, j] = PointDensity(v_map, i, j, map_size);
                }
            }
        }

        public static int CaculateInterpolated_value(int[,] grad_map, int[] grad_map_dim, int[] target_map_dim, int pos_x, int pos_y)
        {
            // we convert the coordinates from 1 map and figure out the equivalent coordinates coordinates on another map , so they might be floating numbers
            float[] ajustedpos = { pos_x * grad_map_dim[0] / (float)target_map_dim[0], pos_y * grad_map_dim[1] / (float)target_map_dim[1] };
            //now we convert them to integers to get the indexes of its neighbors
            int[] rounded_pos = { (int)Math.Round(ajustedpos[0]), (int)Math.Round(ajustedpos[1]) };
            List<int[]> neighbor_indexs = GetSurroundingCoords(ajustedpos, rounded_pos, grad_map_dim , 2);


            const int DISTANCE_INFLUENCE = 2;
            const float DENSITY_MULTIPLAYER = 1f ;
            // now we have the neighboring gradient indexe
            int cpt = 0;
            float total_den = 0;
            while (cpt < neighbor_indexs.Count)
            {
                total_den += grad_map[neighbor_indexs[cpt][0], neighbor_indexs[cpt][1]] / (1 + coord_distance(ajustedpos, neighbor_indexs[cpt]) * DISTANCE_INFLUENCE);
                cpt++;
            }
            neighbor_indexs = null;

            return (int)(total_den * DENSITY_MULTIPLAYER / (1 + cpt));
        }


        public static void InterpaleValues(int[,] grad_map, int[,] den_map)
        {
            int[] grad_map_dim = { grad_map.GetLength(0), grad_map.GetLength(1) };
            int[] den_map_dim = { den_map.GetLength(0), den_map.GetLength(1) };
            for (int x = 0; x < den_map.GetLength(0); x++)
            {
                for (int y = 0; y < den_map.GetLength(1); y++)
                {
                    den_map[x, y] = CaculateInterpolated_value(grad_map, grad_map_dim, den_map_dim, x, y);
                }
            }

        }

        public static double DotProduct(Vector v1, Vector v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }


        public static void Create_vectors(Vector[,] array, Random rand_gen)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    int angle = rand_gen.Next(0, 361);
                    array[i, j] = new Vector((float)Math.Cos(angle), (float) Math.Sin(angle));
                    
                }
            }
        }
        public static void Display_vector_map(Vector[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j].PrintVector();
                }
            }
        }

     
        

        public PerlinNoiseMap(int noise_size_x , int noise_size_y , int noise_detaile , int image_size_y , int Random_seed = 55)
        {
            Random generator = new Random(Random_seed);
            Vector[,] vector_map = new Vector[noise_size_x, noise_size_y];
            int[,] gradient_map = new int[noise_size_x * 2 -2 , noise_size_y * 2 - 2];
            int[,] den_map = new int[noise_size_x * noise_detaile, noise_size_y * image_size_y];

            Create_vectors(vector_map, generator);
            Create_density_map(vector_map, gradient_map);

            
            InterpaleValues(gradient_map, den_map);

            this.noise_map = den_map;

            
        }

    }




    /*
     * <summary>
     * Image_editing class is for editing single static im
     * </summary>
     */
    internal class Image_Editing
    {

        public static bool isValidCoord(int[] coord, int[] current_map_size)
        {
            return (coord[0] >= 0 && coord[1] >= 0 && coord[0] < current_map_size[0] && coord[1] < current_map_size[1]);
        }

        public int sleep_rate { get; set; }
        public int[] GetimageData_range(int[,] Image)
        {
            int max_ = Image[0, 0];
            int min_ = Image[0, 0];
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    max_ = Math.Max(max_, Image[i, j]);
                    min_ = Math.Min(max_, Image[i, j]);
                }

            }
            int[] data_range = { min_, max_ };
            return data_range;
        }
        public void Convert_to_255(int[,] image)
        {
            int Target_range = 255;
            int[] datarange = GetimageData_range(image);
            for (int x = 0; x < image.GetLength(0); x++)
            {
                for(int y = 0; y < image.GetLength(1); y++)
                {
                    image[x, y] = Math.Min((image[x, y] - datarange[0]) * Target_range / datarange[1] ,255);
                }
            }
        }
        
        // given a value between 0 - 255 , what is the best char to represent it?
        public char DeterminerSymbole(int value)
        {
            int[] thresholds = { 30, 60, 90, 120, 150, 180, 240 };
            char[] thres_values = { '.', ',', ':', ';', '-', '+', '#' };
     
            char c = ' ';
            
            int jumpamount = 2;
            int pt;
            for (pt = 3;jumpamount > 0; jumpamount--)
            {
                if(value > thresholds[pt])
                {
                    pt += jumpamount;
                }
                else
                {
                    pt -= jumpamount;
                }
            }
            if(value > thresholds[pt])
            {
                c = thres_values[pt];
            }
            
            
            return c;

        }


        public void RenderLine(int[,] image ,int line)
        {
            string line_message = "";
            for (int y = 0; y < image.GetLength(1) - 1; y++)
            {
      
                    line_message += new string( DeterminerSymbole(image[line, y]) , 2) ;
                
               
                
            }
            
            Console.WriteLine(line_message);
        }


        public void Render_255_BW_image(int[,] image)
        {
      
            for(int x = 0; x < image.GetLength(0); x++)
            {
                RenderLine(image , x);
            }
        }

        // given a image bump the value of all the pixels by a certain amount
        public void Image_bump(int[,] image , int amount)
        {
            
            for (int x = 0; x < image.GetLength(0); x++)
            {
                for (int y = 0; y < image.GetLength(1); y++)
                {
                    image[x, y] = Math.Min(image[x, y] + amount, 255);
                }
            }
        }

        //given two integers x and y , let's suppose now these two sets
        // X = {-x , ... , -1 , 0 , 1 ,.. x }  Y = { - y , ... , -1 , 0 , 1 ,... y}
        // this function returns the list X * Y ,
        // it gives all posibile combination of (X , Y )
        public int[,] produce_offset_ranges(int amount_x, int amount_y)
        {
            int[,] offsets = new int[(amount_x * 2 + 1) * (amount_y * 2 + 1)  , 2];

            int cpt = 0;
            for (int x = -amount_x; x <= amount_x; x++)
            {
                for(int y = -amount_y; y <= amount_y; y++)
                {
                    offsets[cpt, 0] = x;
                    offsets[cpt, 1] = y;
                    cpt++;
                    
                }
            }
            return offsets;
        }

        //given a coordinate of a image and a x , y returns the average value of its neighbores of x distance x , and y distance y 
        public int average_of_neighbors(int[,] image , int pos_x , int pos_y , int amount_x, int amount_y )
        {
            int[] target_map_size = {image.GetLength(0) ,  image.GetLength(1) };
            int average = 0;
            int cpt = 0;
            int[,] NEIGHBOR_OFFSETS = produce_offset_ranges( amount_x, amount_y);
            for (int i = 0; i < NEIGHBOR_OFFSETS.GetLength(0); i++)
            {
                int[] new_cord = { pos_x + NEIGHBOR_OFFSETS[i, 0], pos_y + NEIGHBOR_OFFSETS[i, 1] };
                if (isValidCoord(new_cord, target_map_size))
                {
                    cpt++;
                    average += image[new_cord[0], new_cord[1]]; 
                }
            }
            NEIGHBOR_OFFSETS = null;
            target_map_size = null;
            return average / cpt ;
        }
        public void simple_blure(int[,] image , int amount_x , int amount_y)
        {
            int[] image_dim = {image.GetLength(0) , image.GetLength(1) }; 
            

            for (int x = 0; x < image_dim[0]; x++)
            {
                for (int y = 0; y < image_dim[1]; y++)
                {
                    image[x, y] = Math.Min(average_of_neighbors(image,x, y, amount_x, amount_y) + image[x, y], 255);
                }
            }
            image_dim = null;
            
        }

        public int[] GetImageSize(int[,] image)
        {
            int[] size = { image.GetLength(0), image.GetLength(1) };
            return size;
        }

        public void PrintImageSize(int[,] image)
        {
            int[] size = GetImageSize(image);
            Console.WriteLine($" Image size ---   x : {size[0]}   y : {size[1]}");
            size = null;
        }
        public bool ImageSameSize(int[,] image1 , int[,] image2)
        {
            int[] size1 = GetImageSize(image1);
            int[] size2 = GetImageSize(image2);
            bool result = (size1[0] == size2[0] && size2[1] == size1[1]);
            size2 = null;
            size1 = null;
            return result;
        }


        // this function puts a image2 on top of image 1
        public void Add_images(int[,] image1 , int[,]image2)
        {

            if(ImageSameSize(image1 , image2))
             {
                Console.WriteLine('h');
                for (int x  = 0; x < image1.GetLength(0); x++)
                {
                    for (int y = 0; y < image1.GetLength(0); y++)
                    {
                        image1[x , y] = Math.Min(255 , image1[x, y] + image2[x , y]);
                    }
                }
             }
        }
        

        //given a image and two integers start_y and end_y , the function first renders the line start_y of the image , then advances and repeats
        //till it reached end_y, if the pointer goes above the image size
        public void Partial_Vertical_display(int[,] image , int start_y , int end_y )

        {
            int image_size_y = image.GetLength(0) - 1;
            int y = start_y;
            while (y != end_y)
            {
                RenderLine(image, y);
                if (y >= image_size_y-1)
                {

                    y = 0;
                }
                else
                {
                    y++;
                }
            }
        }


        public int Format_int_to_range(int target, int min, int max)
        {
            return (target - min) % (max - min) + min; 
        }


        public void Frame_Sleep()
        {
            Thread.Sleep(this.sleep_rate);
        }

        public Image_Editing(int framerate)
        {
            this.sleep_rate = 1000 / framerate;
        }

        // given a triange with the points a b c , angle of abc is 90 deg , and given two integers the lenght of a to c and the angle cab
        // return the position of c , supposing a is (0 , 0 )
        public int[] GetCoordFromAngleAndHypoLenght(int angle , int hypo)
        {
            int[] coord = { (int)Math.Round(Math.Sin(angle) * hypo) , (int) Math.Round(Math.Cos(angle) * hypo)};
            return coord;
        }
        //<summary>
        // takes a image of n * m
        // returns a warped circular image of n * m of that image
        //</summary>
        public int[,] CreateCurvedCircularImage(int[,] image)
        {
            int[] image_size = GetImageSize(image);
            int[,] new_image = new int[(image_size[0] * 2) + 2, (image_size[1] * 2) + 2];
            int[] circleMiddleCoord = { image_size[0], image_size[1]};

            for (int i = 0; i < image_size[0]; i++)
            {
                for (int j = 0; j < image_size[1]; j++)
                {
                    int[] OffsetCoord = GetCoordFromAngleAndHypoLenght(j, i);
                    new_image[OffsetCoord[0] + circleMiddleCoord[0], OffsetCoord[1] + circleMiddleCoord[1]] += image[i, j];
                }
            }
            return new_image;
        }


        // I am making this shader to speed up my animation and make it faster ,
        // basically this is CreateCurvedCircularImage() , AddContrast() and Vertical Gradient()
        public int[,] FlareShader(int[,] image)
        {
            int[] image_size = GetImageSize(image);
            int[,] new_image = new int[(image_size[0] * 2) + 2, (image_size[1] * 2) + 2];
            int[] circleMiddleCoord = { image_size[0], image_size[1] };

            float increments = 1.0f / image_size[0];
            float amount = 1.0f;
            for (int i = 0; i < image_size[0]; i++)
            {
                for (int j = 0; j < image_size[1]; j++)
                {
                    int[] OffsetCoord = GetCoordFromAngleAndHypoLenght(j, i);
                    new_image[OffsetCoord[0] + circleMiddleCoord[0], OffsetCoord[1] + circleMiddleCoord[1]] += (int) (Math.Pow(image[i, j] , 1.4f));
       

                }
                amount -= increments;
            }
            return new_image;

        }

        // adds contrast to the image
        public void AddContrast(int[,] image ,float amount)
        {
            int[] image_size = GetImageSize(image);
           
            for (int i = 0; i < image_size[0]; i++)
            {
                for (int j = 0; j < image_size[1]; j++)
                {
                    image[i , j] = Math.Min((int)Math.Round(Math.Pow(image[i , j] , amount)) , 255);
                }
            }
        }


        // given a image copy a partial vertical amount to a new image and return it 
        public int[,] CopyPartialVerticalImage(int[,] image , int verticall_amount  , int start)
        {
            int[,] new_image = new int[verticall_amount, image.GetLength(1)];
            int pointer = start;
            for (int i = 0; i < verticall_amount; i++) 
            {
                for (int j = 0 ; j < image.GetLength(1) ; j++)
                {
                    new_image[i, j] = image[pointer, j];
                }
                if (pointer >= image.GetLength(0) - 1)
                {

                    pointer = 0;
                }
                else
                {
                    pointer++;
                }

            }

            return new_image;
        }
    }
    // woriking on this not ready 
    internal class Layer
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int[,] IMAGE { get; set; }
        public Layer(int x, int y , int dimX , int dimY)
        {
            X = x;
            Y = y;  
            IMAGE = new int[dimY, dimX];
        }

        public int[,] Render(int canvasX , int canvasY)
        {
            return IMAGE;
        }

        
    }

    // working on this not ready 

    internal class Canvas
    {
        public int Size_X { get; set; }
        public int Size_Y { get; set; }

        public List<Layer> Layers { get; set; }
        public Canvas(int sizeX , int sizeY)
        {

        }
    }

    internal class Effects
    {
        public static void Square(int[,] image , int x, int y , int size)
        {
            if(y < size || y > image.GetLength(0) - size)
            {
                Console.WriteLine("y must be between [size , image.GetLenght(0) - size)");
                return;
            }
            if(x < size || x > image.GetLength(1) - size) {
                Console.WriteLine("x must be between [size , image.GetLenght(1) - size)");
                return;
            }
            int stopY = y + size;
            int stopX = x + size;
            for (int pY = y; pY < stopY; pY++)
            {
                for (int pX = x; pX < stopX; pX++)
                {
                    image[pY, pX] = 255;
                } 
            }
        }

        // working on it , not finished
        public static void Circle(int[,] image , int circleX, int circleY, int radius)
        {
            for (int y = 0; y > image.GetLength(0);y++ )
            {
                for(int x = 0; x < image.GetLength(1);x++)
                {

                }
            }
        }
    }

    
}
