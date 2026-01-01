// Auto-generated Gaussian Blur (offset-packed)
// sigma = 12

#ifndef GAUSSIAN_BLUR_INCLUDED
#define GAUSSIAN_BLUR_INCLUDED

static const int GAUSSIAN_TAP_COUNT = 19;

static const float GaussianWeights[GAUSSIAN_TAP_COUNT] = {
    0.033323f,
    0.066072f,
    0.063821f,
    0.059960f,
    0.054793f,
    0.048702f,
    0.042104f,
    0.035404f,
    0.028956f,
    0.023035f,
    0.017823f,
    0.013414f,
    0.009819f,
    0.006991f,
    0.004841f,
    0.003261f,
    0.002137f,
    0.001361f,
    0.000844f,
};

static const float GaussianOffsets[GAUSSIAN_TAP_COUNT] = {
    0.000000f,
    1.497396f,
    3.493924f,
    5.490453f,
    7.486982f,
    9.483513f,
    11.480050f,
    13.476580f,
    15.473120f,
    17.469660f,
    19.466200f,
    21.462740f,
    23.459290f,
    25.455840f,
    27.452400f,
    29.448960f,
    31.445530f,
    33.442100f,
    35.438680f,
};

#endif
