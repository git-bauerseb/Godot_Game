shader_type canvas_item;

uniform vec2 mouse;

float smin(float a, float b, float k) {
	float h = clamp(0.5 * 0.5*(b-a)/k, 0.0, 1.0);
	return mix(b,a,h) - k*h*(1.0-h);
}

float smin2(float a, float b, float k )
{
    float res = exp2( -k*a ) + exp2( -k*b );
    return -log2( res )/k;
}

float circle(vec2 coord, vec2 center, float r) {
	return 1.0- step(0.1, distance(coord, center));
}

/*
vec3 map(vec2 coord, vec3 colorA, vec3 colorB) {
	

	vec3 circ1 = (distance(coord, mouse) - 0.1) * colorA;
	vec3 circ2 = (distance(coord, vec2(0.2, 0.4)) - 0.2) * colorB;

	// return smin2(circ2, circ1, 32);
	//return min(circ1, circ2);
}
*/

vec3 map(vec2 coord, vec3 colorA, vec3 colorB) {
	float circ1 = (distance(coord, mouse) - 0.1);
	float circ2 = (distance(coord, vec2(0.2, 0.4)) - 0.2);

	float sum = abs(circ1) + abs(circ2);

	return abs(circ1)/sum * colorA + abs(circ2)/sum * colorB;
}

void fragment() {

	vec2 coord = UV;

	vec3 colorA = vec3(0.8, 0.2, 0.2);
	vec3 colorB = vec3(0.2, 0.8, 0.2);
	vec3 colorC = vec3(0.2, 0.8, 0.8);
	vec3 colorD = vec3(1.0);

	vec3 color = map(coord, colorA, colorB);
	// color += fract(max(vec3(0.0), map(coord))*10.0);
	COLOR = vec4(color, 1.0);
}