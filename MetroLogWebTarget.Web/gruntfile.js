/// <binding BeforeBuild='jshint:all' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-jshint');

    grunt.initConfig({
        uglify: {
            my_target: {
                files: { 'wwwroot/app.js': ['Scripts/app.js', 'Scripts/src/**/*.js'] }
            }
        },

        watch: {
            scripts: {
                files: ['Scripts/src/**/*.js'],
                tasks: ['uglify']
            }
        },
        
        jshint: {
            all: ['gruntfile.js', 'Scripts/src/**/*.js']
        }
    });

    grunt.registerTask('default', ['uglify', 'watch', "jshint"]);
};