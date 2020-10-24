xof 0302txt 0064
template Header {
 <3D82AB43-62DA-11cf-AB39-0020AF71E433>
 WORD major;
 WORD minor;
 DWORD flags;
}

template Vector {
 <3D82AB5E-62DA-11cf-AB39-0020AF71E433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template Coords2d {
 <F6F23F44-7686-11cf-8F52-0040333594A3>
 FLOAT u;
 FLOAT v;
}

template Matrix4x4 {
 <F6F23F45-7686-11cf-8F52-0040333594A3>
 array FLOAT matrix[16];
}

template ColorRGBA {
 <35FF44E0-6C7C-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <D3E16E81-7835-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template IndexedColor {
 <1630B820-7842-11cf-8F52-0040333594A3>
 DWORD index;
 ColorRGBA indexColor;
}

template Boolean {
 <4885AE61-78E8-11cf-8F52-0040333594A3>
 WORD truefalse;
}

template Boolean2d {
 <4885AE63-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template MaterialWrap {
 <4885AE60-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template TextureFilename {
 <A42790E1-7810-11cf-8F52-0040333594A3>
 STRING filename;
}

template Material {
 <3D82AB4D-62DA-11cf-AB39-0020AF71E433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshFace {
 <3D82AB5F-62DA-11cf-AB39-0020AF71E433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template MeshFaceWraps {
 <4885AE62-78E8-11cf-8F52-0040333594A3>
 DWORD nFaceWrapValues;
 Boolean2d faceWrapValues;
}

template MeshTextureCoords {
 <F6F23F40-7686-11cf-8F52-0040333594A3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template MeshMaterialList {
 <F6F23F42-7686-11cf-8F52-0040333594A3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material]
}

template MeshNormals {
 <F6F23F43-7686-11cf-8F52-0040333594A3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template MeshVertexColors {
 <1630B821-7842-11cf-8F52-0040333594A3>
 DWORD nVertexColors;
 array IndexedColor vertexColors[nVertexColors];
}

template Mesh {
 <3D82AB44-62DA-11cf-AB39-0020AF71E433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

Header{
1;
0;
1;
}

Mesh {
 44;
 -0.25000;0.25000;-1.00000;,
 -0.25000;-0.25000;-1.00000;,
 -0.50000;-0.17500;-1.00000;,
 -0.50000;0.17500;-1.00000;,
 -0.25000;0.25000;1.50000;,
 -0.25000;-0.25000;1.50000;,
 -0.50000;0.17500;1.50000;,
 -0.50000;-0.17500;1.50000;,
 -0.75000;0.50000;-1.50000;,
 -0.75000;-0.50000;-1.50000;,
 -1.00000;-0.42500;-1.50000;,
 -1.00000;0.42500;-1.50000;,
 -0.75000;0.50000;0.75000;,
 -0.75000;-0.50000;0.75000;,
 -1.00000;0.42500;0.75000;,
 -1.00000;-0.42500;0.75000;,
 0.50000;-0.17500;-1.00000;,
 0.25000;-0.25000;-1.00000;,
 0.25000;0.25000;-1.00000;,
 0.50000;0.17500;-1.00000;,
 0.25000;-0.25000;1.50000;,
 0.25000;0.25000;1.50000;,
 0.50000;-0.17500;1.50000;,
 0.50000;0.17500;1.50000;,
 1.00000;-0.42500;-1.50000;,
 0.75000;-0.50000;-1.50000;,
 0.75000;0.50000;-1.50000;,
 1.00000;0.42500;-1.50000;,
 0.75000;-0.50000;0.75000;,
 0.75000;0.50000;0.75000;,
 1.00000;-0.42500;0.75000;,
 1.00000;0.42500;0.75000;,
 0.00000;0.25000;0.00000;,
 0.25000;-0.00000;0.00000;,
 0.00000;0.00000;-0.25000;,
 -0.00000;0.00000;0.25000;,
 -0.25000;0.00000;0.00000;,
 0.00000;-0.25000;-0.00000;,
 0.00000;0.31250;0.00000;,
 0.00000;0.00000;-0.31250;,
 0.31250;0.00000;0.00000;,
 0.00000;0.00000;0.31250;,
 -0.31250;0.00000;0.00000;,
 0.00000;-0.31250;-0.00000;;
 
 64;
 3;0,1,2;,
 3;0,2,3;,
 3;4,5,1;,
 3;4,1,0;,
 3;6,7,5;,
 3;6,5,4;,
 3;3,2,7;,
 3;3,7,6;,
 3;4,0,3;,
 3;4,3,6;,
 3;1,5,7;,
 3;1,7,2;,
 3;8,9,10;,
 3;8,10,11;,
 3;12,13,9;,
 3;12,9,8;,
 3;14,15,13;,
 3;14,13,12;,
 3;11,10,15;,
 3;11,15,14;,
 3;12,8,11;,
 3;12,11,14;,
 3;9,13,15;,
 3;9,15,10;,
 3;16,17,18;,
 3;19,16,18;,
 3;17,20,21;,
 3;18,17,21;,
 3;20,22,23;,
 3;21,20,23;,
 3;22,16,19;,
 3;23,22,19;,
 3;19,18,21;,
 3;23,19,21;,
 3;22,20,17;,
 3;16,22,17;,
 3;24,25,26;,
 3;27,24,26;,
 3;25,28,29;,
 3;26,25,29;,
 3;28,30,31;,
 3;29,28,31;,
 3;30,24,27;,
 3;31,30,27;,
 3;27,26,29;,
 3;31,27,29;,
 3;30,28,25;,
 3;24,30,25;,
 3;32,33,34;,
 3;32,35,33;,
 3;32,36,35;,
 3;32,34,36;,
 3;34,33,37;,
 3;33,35,37;,
 3;35,36,37;,
 3;36,34,37;,
 3;38,39,40;,
 3;38,40,41;,
 3;38,41,42;,
 3;38,42,39;,
 3;39,43,40;,
 3;40,43,41;,
 3;41,43,42;,
 3;42,43,39;;
 
 MeshMaterialList {
  4;
  64;
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  3,
  3,
  3,
  3,
  3,
  3,
  3,
  3,
  2,
  2,
  2,
  2,
  2,
  2,
  2,
  2;;
  Material {
   0.768627;0.768627;0.768627;0.750000;;
   5.000000;
   0.500000;0.500000;0.500000;;
   0.000000;0.000000;0.000000;;
  }
  Material {
   0.800000;0.800000;0.800000;0.750000;;
   5.000000;
   0.750000;0.750000;0.750000;;
   0.500000;0.500000;0.500000;;
  }
  Material {
   0.000000;1.000000;0.000000;1.000000;;
   5.000000;
   1.000000;1.000000;1.000000;;
   0.000000;0.250000;0.000000;;
  }
  Material {
   1.000000;0.000000;1.000000;1.000000;;
   5.000000;
   1.000000;1.000000;1.000000;;
   0.250000;0.000000;0.250000;;
  }
 }
 MeshNormals {
  32;
  0.000000;0.000000;-1.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.000000;1.000000;,
  -1.000000;0.000000;0.000000;,
  -0.287348;0.957826;0.000000;,
  -0.287348;-0.957826;0.000000;,
  -0.287348;0.957826;0.000000;,
  -0.287348;0.957826;0.000000;,
  -0.287348;-0.957826;0.000000;,
  -0.287348;-0.957826;0.000000;,
  0.287348;0.957826;0.000000;,
  0.287348;-0.957826;-0.000000;,
  0.287348;0.957826;0.000000;,
  0.287348;0.957826;0.000000;,
  0.287348;-0.957826;-0.000000;,
  0.287348;-0.957826;-0.000000;,
  0.577350;0.577350;-0.577350;,
  0.577350;0.577350;0.577350;,
  -0.577350;0.577350;0.577350;,
  0.577350;-0.577350;-0.577350;,
  -0.577350;0.577350;-0.577350;,
  0.577350;-0.577350;0.577350;,
  -0.577350;-0.577350;0.577350;,
  -0.577350;-0.577350;-0.577350;,
  -0.577350;-0.577350;0.577350;,
  -0.577350;-0.577350;-0.577350;,
  0.577350;-0.577350;-0.577350;,
  -0.577350;0.577350;0.577350;,
  0.577350;-0.577350;0.577350;,
  -0.577350;0.577350;-0.577350;,
  0.577350;0.577350;-0.577350;,
  0.577350;0.577350;0.577350;;
  64;
  3;0,0,0;,
  3;0,0,0;,
  3;1,1,1;,
  3;1,1,1;,
  3;2,2,2;,
  3;2,2,2;,
  3;3,3,3;,
  3;3,3,3;,
  3;4,4,4;,
  3;4,4,4;,
  3;5,5,5;,
  3;5,5,5;,
  3;0,0,0;,
  3;0,0,0;,
  3;1,1,1;,
  3;1,1,1;,
  3;2,2,2;,
  3;2,2,2;,
  3;3,3,3;,
  3;3,3,3;,
  3;6,7,6;,
  3;6,6,7;,
  3;8,9,8;,
  3;8,8,9;,
  3;0,0,0;,
  3;0,0,0;,
  3;3,3,3;,
  3;3,3,3;,
  3;2,2,2;,
  3;2,2,2;,
  3;1,1,1;,
  3;1,1,1;,
  3;10,10,10;,
  3;10,10,10;,
  3;11,11,11;,
  3;11,11,11;,
  3;0,0,0;,
  3;0,0,0;,
  3;3,3,3;,
  3;3,3,3;,
  3;2,2,2;,
  3;2,2,2;,
  3;1,1,1;,
  3;1,1,1;,
  3;12,13,12;,
  3;13,12,12;,
  3;14,15,14;,
  3;15,14,14;,
  3;16,16,16;,
  3;17,17,17;,
  3;18,18,18;,
  3;20,20,20;,
  3;19,19,19;,
  3;21,21,21;,
  3;22,22,22;,
  3;23,23,23;,
  3;24,24,24;,
  3;25,25,25;,
  3;26,26,26;,
  3;28,28,28;,
  3;27,27,27;,
  3;29,29,29;,
  3;30,30,30;,
  3;31,31,31;;
 }
 MeshVertexColors {
  44;
  0;1.000000;1.000000;1.000000;1.000000;,
  1;1.000000;1.000000;1.000000;1.000000;,
  2;1.000000;1.000000;1.000000;1.000000;,
  3;1.000000;1.000000;1.000000;1.000000;,
  4;1.000000;1.000000;1.000000;1.000000;,
  5;1.000000;1.000000;1.000000;1.000000;,
  6;1.000000;1.000000;1.000000;1.000000;,
  7;1.000000;1.000000;1.000000;1.000000;,
  8;1.000000;1.000000;1.000000;1.000000;,
  9;1.000000;1.000000;1.000000;1.000000;,
  10;1.000000;1.000000;1.000000;1.000000;,
  11;1.000000;1.000000;1.000000;1.000000;,
  12;1.000000;1.000000;1.000000;1.000000;,
  13;1.000000;1.000000;1.000000;1.000000;,
  14;1.000000;1.000000;1.000000;1.000000;,
  15;1.000000;1.000000;1.000000;1.000000;,
  16;1.000000;1.000000;1.000000;1.000000;,
  17;1.000000;1.000000;1.000000;1.000000;,
  18;1.000000;1.000000;1.000000;1.000000;,
  19;1.000000;1.000000;1.000000;1.000000;,
  20;1.000000;1.000000;1.000000;1.000000;,
  21;1.000000;1.000000;1.000000;1.000000;,
  22;1.000000;1.000000;1.000000;1.000000;,
  23;1.000000;1.000000;1.000000;1.000000;,
  24;1.000000;1.000000;1.000000;1.000000;,
  25;1.000000;1.000000;1.000000;1.000000;,
  26;1.000000;1.000000;1.000000;1.000000;,
  27;1.000000;1.000000;1.000000;1.000000;,
  28;1.000000;1.000000;1.000000;1.000000;,
  29;1.000000;1.000000;1.000000;1.000000;,
  30;1.000000;1.000000;1.000000;1.000000;,
  31;1.000000;1.000000;1.000000;1.000000;,
  32;1.000000;1.000000;1.000000;1.000000;,
  33;1.000000;1.000000;1.000000;1.000000;,
  34;1.000000;1.000000;1.000000;1.000000;,
  35;1.000000;1.000000;1.000000;1.000000;,
  36;1.000000;1.000000;1.000000;1.000000;,
  37;1.000000;1.000000;1.000000;1.000000;,
  38;1.000000;1.000000;1.000000;1.000000;,
  39;1.000000;1.000000;1.000000;1.000000;,
  40;1.000000;1.000000;1.000000;1.000000;,
  41;1.000000;1.000000;1.000000;1.000000;,
  42;1.000000;1.000000;1.000000;1.000000;,
  43;1.000000;1.000000;1.000000;1.000000;;
 }
}
