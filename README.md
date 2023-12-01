At the time this was for making noise but this project evolved for making special effects and editing images



<br />
<br />
<br />
<br />
<br />
<br />

demo:
if you past this code into your main program you will get a rolling cloud animation

<br />
<br />
//C#

using Editor;

<br />
<br />

const int FRAMERATE = 60;

<br />
<br />


<br />
<br />

<br />
<br />





<br />
<br />


<br />
<br />

const int NOISE_SIZE_X = 10,
          NOISE_SIZE_Y = 10,
          IMAGE_DETAIL_X = 6,
          IMAGE_DETAIL_Y = 6,
          RANDOM_SEED = 36;


PerlinNoiseMap perlin_n_map = new PerlinNoiseMap(NOISE_SIZE_X, NOISE_SIZE_Y, IMAGE_DETAIL_X, IMAGE_DETAIL_Y, RANDOM_SEED);


Image_Editing editor = new Image_Editing(FRAMERATE);

const int MAX_FRAME = 2000;
const int window_gap = 50;

int image_size = editor.GetImageSize(perlin_n_map.noise_map)[0];

const int SPEED = 1;

for (int frame_number = 1; frame_number < MAX_FRAME; frame_number += SPEED)
{
   

    int start = editor.Format_int_to_range(MAX_FRAME - frame_number, 0, image_size);


    int[,] partialmap = editor.CopyPartialVerticalImage(perlin_n_map.noise_map, window_gap, start);
    
    
    Console.Clear();
    editor.Render_255_BW_image(partialmap);
    editor.Frame_Sleep();
}



     ..
     .,.
     ...
       .         .
         ..     .,..
         ...    .,..
        ..,     .:,.
                .:,..
                .:,,...
                .,,,,,,,.
                .,,,,,:,,.
                 ,,,,::,..
                .,,,:::,.
                 .,,,:,,.               
                ...,,:,,.
                 ...,,,.








![demo](https://github.com/SavageDud/Noise_generator/assets/67841707/bfbe8576-37e6-41c7-877d-173ba244d110)

<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />

another is adding one line and you get a firework:


for (int frame_number = 1; frame_number < MAX_FRAME; frame_number += SPEED)
{
   

    int start = editor.Format_int_to_range(MAX_FRAME - frame_number, 0, image_size);


    int[,] partialmap = editor.CopyPartialVerticalImage(perlin_n_map.noise_map, window_gap, start);

    
    //this line right here
    partialmap = editor.FlareShader(partialmap);

    
    Console.Clear();
    editor.Render_255_BW_image(partialmap);
    editor.Frame_Sleep();
}

![firework](https://github.com/SavageDud/Noise_generator/assets/67841707/ceaed76d-9fe5-4dca-af09-7221963a5e2a)



<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />


Documentation: 

new PerlinNoiseMap(NOISE_SIZE_X, NOISE_SIZE_Y, IMAGE_DETAIL_X, IMAGE_DETAIL_Y, RANDOM_SEED);

//create a noise map 
// the int[,] object of the map is in PerlinNoiseMap.noise_map

<br />
<br />
<br />
<br />
<br />
<br />






// now the Image_Editing class
// the main function are 
<br />
<br />

Image_Editing.Convert_to_255(int[,] image);// converts each pixel to a range of 0-255 value
<br />
<br />

Image_Editing.Render_255_BW_image(int[,] image);// renders a int[,] object as a image
<br />
<br />

Image_Editing.Image_bump(int[,] image , int amount);// adds the value <amount> to each pixel of the image
<br />
<br />

Image_Editing.simple_blure(int[,] image , int amount_x , int amount_y ); // blures the image by amount_x horizontally and amount_y vertically
<br />
<br />

Image_Editing.Add_images(int[,] image1 , int[,] image2); // overlays image2 on top of image1
<br />
<br />

Image_Editing.Partial_Vertical_display(int[,] image , int start_y , int end_y ); // renders the image from start_y to to end_y , end_y can be smaller than start_y
<br />
<br />

Image_Editing.CreateCurvedCircularImage(int[,] image); // image putting a point at y = 0 and x half the lenght of the image , then wraping the entir image around that point , then returning the new image
<br />
<br />
Image_Editing.AddContrast(int[,] image , float amount) // raise the value of each pixel to the power of <amount>

<br />
<br />

Image_Editing.CopyPartialVerticalImage(int[,] image , int verticall_amount  , int start) 
// same as Partial_Vertical_display() but returns it rather than rendering it , also you need to give it a start and 
//instead of a end you give it a amount






// now the Effects class
//

Effects.Square( Square(int[,] image , int x, int y , int size)
// draws a square on the image at the coords x , y of size
