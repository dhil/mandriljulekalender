publish: index.html static js
	git checkout gh-pages
	git checkout master -- index.html
	git checkout master -- static
	git checkout master -- js
	git checkout master
