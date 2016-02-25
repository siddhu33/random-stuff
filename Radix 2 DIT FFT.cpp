// Radix 2 DIT FFT.cpp : Defines the entry point for the console application.
//

#include <iostream>
#include <math.h>
#include <complex>
#include <vector>
#include <time.h>

const double pi = 3.14159265358979323846;

using namespace std;

bool power_of_2(int input){
	return (input != 0) && ((input & (input - 1)) == 0); //if input is a power of 2 and not 0
}

vector<complex<double>> DFT(vector<complex<double>> input, vector<complex<double>> twiddle, int size){
	vector<complex<double>> output;
	for (int j = 0; j < size; j++){
		complex<double> sum(0, 0);
		for (int i = 0; i < size; i++){
			sum += input[i] * twiddle[(i*j) % size];
		}
		output.push_back(sum);
	}
	return output;
}

vector<complex<double>> FFT(vector<complex<double>> input,vector<complex<double>> &twiddle, int size){
	vector<complex<double>> out(size);
	if (size == 2){
		out[0] = (input[0] + input[1]);
		out[1] = (input[0] - input[1]);
		return out;
	}
	else{
		vector<complex<double>> odd;
		vector<complex<double>> even;
		for (int i = 0; i < size; i++){
			if (i % 2 == 0){
				even.push_back(input[i]);
			}
			else{
				odd.push_back(input[i]);
			}
		}
		vector<complex<double>> f_odd = FFT(odd,twiddle,size/2);
		vector<complex<double>> f_even = FFT(even,twiddle,size/2);
		for (int i = 0; i < size; i++){
			out[i] = (f_even[i % (size/2)] + twiddle[i] * f_odd[i % (size/2)]);
		}
		return out;
	}
}
vector<complex<double>> FT(vector<complex<double>> input, int speed){
	int size = input.size();
	if (size == 1){
		return input;
	}
	vector<complex<double>> twiddle(size);
	for (int i = 0; i<size; i++){
		complex<double> basis(0, -(2 * i * pi) / size);
		complex<double> temp = exp(basis);
		twiddle[i] = temp;
	}
	if (power_of_2(size) && speed == 1){
		return FFT(input,twiddle,size);
	}
	else{
		//have to do standard dft due to either speed toggle or not power of 2.
		return DFT(input,twiddle,size);
	}
}




int main(int argc, char* argv[]){
	clock_t t;
	clock_t u;
	vector<complex<double>> temp;
	for (int i = 0; i < 256; i++){
		temp.push_back(complex<double>(cos(2*pi*i/256), 0));
	}
	vector<complex<double>> out = FT(temp, 1);
	vector<float> abs;
	for (int i = 0; i < out.size(); i++){
		abs.push_back(sqrt(pow(out[i].real(), 2) + pow(out[i].imag(), 2)));
	}
	for (int i = 0; i < abs.size(); i++){
		for (int j = 0; j < (int)abs[i]; j++){
			cout << "*";
		}
		cout << "\n";
	}
	/*t = clock();
	vector<complex<double>> out = FT(temp, 1);
	t = clock() - t;
	cout << "FFT = " << (float)t / CLOCKS_PER_SEC << endl;
	u = clock();
	vector<complex<double>> out2 = FT(temp, 0);
	u = clock() - u;
	cout << "DFT = " << (float)u / CLOCKS_PER_SEC << endl;
	return 0;*/
}

