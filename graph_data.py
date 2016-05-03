#import matplotlib
# Force matplotlib to not use any Xwindows backend.
#matplotlib.use('Agg')
import matplotlib.pyplot as plt
import numpy as np
from decimal import Decimal
from datetime import datetime
import sys
import re

#Input stream needs to be of the form: "N={} {Application Name} {time taken}

def main():
	N = set()
	time = {}
        frames = 1
	title = ""
	for line in sys.stdin:
		if("Latency" in line):
			title = "Latency"
		elif("Throughput" in line):
			val = line.split()
			title = val[0]
			frames = int(val[1])
		else:
			values = line.split()
			n = int(values[0][2:])
			N.add(n)
			t = Decimal(values[2])
			name = values[1]
			if name in time:
				time[name].append(t/1000000000)
			else:
				time[name] = []
				time[name].append(t/1000000000)
	ylab = 'Latency (seconds)'
	if("Throughput" in title):
		ylab = 'Throughput (Operations per second)'
		time.update((x,[frames/z for z in y]) for x, y in time.items())
	for k in time:
		plt.plot(sorted(N),time[k])
	plt.xlabel('X axis')
	plt.ylabel(ylab)
	plt.legend(time.keys())
	plt.yscale('log')
	plt.grid(b=True, which='major', color='b', linestyle='-')
	plt.grid(b=True, which='minor', color='r', linestyle='--')
	plt.title('Performance [' + title + ']')
	fig = plt.gcf()
	fig.set_size_inches(19.2, 10.8)
	d = datetime.now()
	fname = title + 'Performance' + d.strftime("%Y%m%dT%H%M%S") + '.png'
	fig.savefig(fname, dpi=100)
	print "Done." 
main()
