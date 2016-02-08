
describe('testthetest', function() {
    var str = "should be";
    beforeEach(function() {
        str = "aaaaa";
    });

    it('should equal', function() {
        expect(str).toEqual('aaaaa');
    });
});