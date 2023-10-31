


using Clouds;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics;













const int FRAMERATE = 10;

const int NOISE_SIZE_X = 8,
          NOISE_SIZE_Y = 8,
          IMAGE_DETAIL_X = 4,
          IMAGE_DETAIL_Y = 4,
          RANDOM_SEED = 76;


PerlinNoiseMap perlin_n_map = new PerlinNoiseMap(NOISE_SIZE_X, NOISE_SIZE_Y, IMAGE_DETAIL_X, IMAGE_DETAIL_Y, RANDOM_SEED);



Image_Editing editor = new Image_Editing(FRAMERATE);



editor.Convert_to_255(perlin_n_map.noise_map);
editor.Render_255_BW_image(perlin_n_map.noise_map);


/*
const int MAX_FRAME = 1000;
const int window_gap = 50;


int image_size = editor.getImageSize(perlin_n_map.noise_map)[0];



for (int frame_number = 1; frame_number < MAX_FRAME; frame_number++)
{
    Console.Clear();
    int start = editor.Format_int_to_range(frame_number, 0, image_size);
    int end = editor.Format_int_to_range(frame_number + window_gap , 0 , image_size);

    editor.Partial_Vertical_display(perlin_n_map.noise_map, start, end , image_size);
    editor.Frame_Sleep();
}


*/
editor = null;
perlin_n_map = null;