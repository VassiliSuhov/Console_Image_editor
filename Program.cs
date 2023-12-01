


using Clouds;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics;











//example 1

/*

const int FRAMERATE = 60;

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



*/







//example 2

/*

const int FRAMERATE = 60;

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

    editor.FlareShader(partialmap);
    Console.Clear();
    editor.Render_255_BW_image(partialmap);
    editor.Frame_Sleep();
}



*/
