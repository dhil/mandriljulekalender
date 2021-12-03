publish: index.html static js
	git checkout gh-pages
	git checkout master -- index.html
	git checkout master -- static
	git checkout master -- js
	git add index.html static/ js/
	git commit -m "Publish application update"
	git push origin gh-pages
	git checkout master
