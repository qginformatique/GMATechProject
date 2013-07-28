cd ..

git clone https://github.com/taobao/nginx-http-concat

svn co svn://svn.nginx.org/nginx/trunk Nginx

cd Nginx

./auto/configure --sbin-path=/usr/local/sbin --with-http_ssl_module --with-http_perl_module --with-http_gzip_static_module --add-module=~/home/gildas/Projects/nginx-http-concat

make

sudo make install

