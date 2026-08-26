shader_type canvas_item;

uniform vec4 color_left : source_color = vec4(0.1, 0.2, 0.3, 1.0);
uniform vec4 color_right : source_color = vec4(0.4, 0.6, 0.8, 1.0);
uniform float speed = 0.5;

void fragment() {
    float factor = sin(UV.x * 3.14159 + TIME * speed) * 0.5 + 0.5;
    
    COLOR = mix(color_left, color_right, factor);
}