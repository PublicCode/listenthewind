$(function(){
	
		$(".select-con select").change(function() {
			var val = $(this).find("option:selected").text()
			$(this).siblings("span").text(val)

		});
		$(".ipts select").change(function() {
			var val = $(this).find("option:selected").text()
			$(this).siblings("span").text(val)

		});
		
		$(".tab-nav a").click(function(){
			var index = $(".tab-nav a").index(this);
			$(this).addClass("crr").siblings().removeClass("crr")
			$(".tab-box > div").eq(index).show().siblings().hide();
			
		});
		$(".ck").click(function(){
			
				$(this).addClass("ckc").siblings().removeClass("ckc")	
			
		});
		$(".ch-list .kq").click(function(){
			
			if($(this).hasClass("kq-c")){
				
				$(this).removeClass("kq-c").parent().siblings(".ch-con").slideDown()
			}else{
				$(this).addClass("kq-c").parent().siblings(".ch-con").slideUp()
			}
		})
		$(".dxuan li").click(function(){
			
			if($(this).hasClass("rddc")){
				
				$(this).removeClass("rddc");
			}else{
				$(this).addClass("rddc");
			}
		})
		$("#srch1").focus(function() {
                    if ($(this).val() == "请输入你要搜索的关键词") {
                        $(this).val("");
						$(this).css("color","#000")

                    }
                }).blur(function() {
                    if ($(this).val() == "") {
                        $(this).val("请输入你要搜索的关键词");
						$(this).css("color","#959596")

                    }

           });
		   $("#srch2").focus(function() {
                    if ($(this).val() == "请输入您要搜索的关键字") {
                        $(this).val("");
						$(this).css("color","#000")

                    }
                }).blur(function() {
                    if ($(this).val() == "") {
                        $(this).val("请输入您要搜索的关键字");
						$(this).css("color","#959596")

                    }

           });
	<!--最新-->
		$(".open-guan").click(function(){
			if($(this).hasClass("crr")){
				
				$(this).removeClass("crr");
				$(this).parent().parent(".info-dd-list").removeClass("g-bi");
				$(this).parent().siblings(".height-auto").show();
				
				
			}else{
				
				$(this).addClass("crr");
				$(this).parent().parent(".info-dd-list").addClass("g-bi");
				$(this).parent().siblings(".height-auto").hide();
			}
			
		});
		$("#xg-Mail").click(function(){
			
			$(this).parent().siblings("input").removeAttr("disabled").addClass("user-wt");
			$(this).parent().siblings("input").focus();
			$(this).hide().siblings("a").show();
		})
		$("#xg-phone").click(function(){
			
			$(this).parent().siblings("input").removeAttr("disabled").addClass("user-wt");
			$(this).parent().siblings("input").focus();
			$(this).hide().siblings("a").show();
		})
		$("#qr-mail").click(function(){
			
			if(isEmail($("#mail").val())){
				$("#mail").attr("disabled","disabled").removeClass("user-wt");
				$(this).hide();
				$("#xg-Mail").show();
			}else{
				alert("邮箱格式不对");
				$("#mail").focus();
			}
			
		})
		$("#qr-phone").click(function(){
			
			if(isMobile($("#phone").val())){
				$("#phone").attr("disabled","disabled").removeClass("user-wt");
				$(this).hide();
				$("#xg-phone").show();
			}else{
				alert("手机号码格式不对");
				$("#phone").focus();
			}
			
		});
		$(".input-list-txt li .nv").click(function(){
			if($(this).hasClass("crr")){

			}else{
				$(this).addClass("crr").siblings().removeClass("crr");
			}	
			
		})
		
	<!--最新-->
})

<!--最新-->
function isEmail(str) {
    var myReg = /^[-._A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/;
    if (myReg.test(str)) {
        return true;
    }
    return false;
}

function isMobile(tel)
{
    var reg = /^0?1[3|4|5|8][0-9]\d{8}$/;
    if (reg.test(tel)) {
        return true;
    } else {
        return false;
    }
}
<!--最新-->