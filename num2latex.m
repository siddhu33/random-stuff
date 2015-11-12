function [str] = num2latex(x)
%mat2latex prints out and stores the format string associated with a lateX
%matrix from a MATLAB array.
str = '\n\\begin{bmatrix}\n';
cell = num2cell(x,2);
for i=1:size(cell,1)
    a = strtrim(num2str(cell{i}));
    a = regexprep(a, '\s+', '&');
    a = strcat(a,'\\\\\n ');
    str = strcat(str,a);
end
str = strcat(str,'\\end{bmatrix}\n');
fprintf(str);
end
