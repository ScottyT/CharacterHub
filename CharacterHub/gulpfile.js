/// <binding AfterBuild='default' />
var gulp = require("gulp"),
    sass = require("gulp-sass"),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream"),
	rename = require("gulp-rename"),
    concat = require("gulp-concat"),
	fontawesome = require("fontawesome-free");

var stylesDir = "Content/Styles/",
	vendorDir = "Content/Vendor/",
	scriptsDir = "Content/Scripts/",
	fontsDir = "Content/webfonts";

var cssFiles = [
	vendorDir + "bootstrap/css/bootstrap.css",
	"node_modules/fontawesome-free/css/all.css"
];

gulp.task("scss", function () {
	var sassStream = gulp.src(stylesDir + "site.scss")
		.pipe(sass({ style: "expanded" }));

	var cssStream = gulp.src(cssFiles);

	return merge(cssStream, sassStream)
		.pipe(concat("site.css"))
		.pipe(gulp.dest(stylesDir));
});

var jsFiles = [
	vendorDir + "jquery/jquery.js",
	vendorDir + "jqueryui/jquery-ui.min.js",
	vendorDir + "jquery/jquery.validate.min.js",
	vendorDir + "jquery/jquery.validate.unobtrusive.min.js",
	vendorDir + "bootstrap/js/bootstrap.js",
	scriptsDir + "character.main.js"
];

gulp.task("js", function () {
	return gulp.src(jsFiles)
		
		.pipe(concat("site.js"))
		//.pipe(gulp.dest(scriptsDir))
		//.pipe(uglify())
		//.pipe(rename({ suffix: '.min' }))
		.pipe(gulp.dest(scriptsDir));
});

gulp.task('fonts', function () {
	return gulp.src('node_modules/fontawesome-free/webfonts/*')
		.pipe(gulp.dest(fontsDir));
});

gulp.task("watch", function () {
	gulp.watch([scriptsDir + "*.js", "!" + scriptsDir + "site.js", "!" + scriptsDir + "site.min.js"], ["js"]);
	gulp.watch(stylesDir + "**/*.scss", ["scss"]);
});

gulp.task('default', ['scss', 'js']);