// underscore.js template config
if (window._ != undefined) {
	_.templateSettings = {
		interpolate: /\{\{(.+?)\}\}/g,
		evaluate: /\@\@(.+?)\@\@/g
	};
}

// jquery and jqueryGrid is necessary
$.expr[':'].required = function (e) {
    return e.required === true;
}
$.expr[':'].readonly = function (e) {
    return e.readOnly === true;
}

(function (jq) {
	jq.fn.extend({
		json: function () {
			var data = {};
			if (this.length == 0) return data;
			if (this.serializeArray) {
				$.each(this.serializeArray(), function () {
					data[this.name] = this.value;
				});
			}
			return data;
		},
		loading: function () {
			if (this.length == 0) return;
			var bound = null;
			if (this[0] == document || this[0] == window) {
				bound = document.body.getBoundingClientRect();
			} else {
				bound = this[0].getBoundingClientRect();
			}
			var loadingPanel = $("<div class='loadingPanel'></div>");
			loadingPanel.css({
				left: bound.left,
				top: bound.top,
				width: bound.right - bound.left,
				height: bound.bottom - bound.top,
				position: "absolute",
				"background-image": "url('images/loading.gif')",
				"background-position": "50% 50%",
				"background-repeat": "no-repeat",
				"background-color": "rgba(0,0,0,0.5)",
				"z-index": 1000
			});
			$(document.body).append(loadingPanel);
			return loadingPanel;
		},
		model: function (obj) {
			if (obj) {
				$.each(this, function () {
					if (this.tagName.toLowerCase == "form") {
						var ctrls = $(this).find("input,select,textarea");
						$.each(ctrls, function () {
							var ele = $(this);
							if (this.name) {
								ele.val(obj[this.name] || "");
							}
						});
					}
				});
			}
			return this;
		},
		modelEx: function (obj) {
			if (obj) {
				for (var k in obj) {
					var e = $("*[data-viewmodel=" + k + "]");
					if (e.length > 0) {
						$.each(e, function () {
							var t = this.tagName.toLowerCase();
							var v=obj[k];
							if(v) {
                                var fmt = $(this).data("modelformat"); // 自定义渲染格式
                                if (fmt) {
                                	if(fmt=="unixtimestamp"){
                                		var timestamp=parseInt(v);
                                		if(!isNaN(timestamp)) {
                                            v = new Date(parseInt(timestamp)).Format($(this).data("format"));
                                        }
									}
                                }
                            }
							if (t == "input" || t == "select" || t == "textarea") {
								$(this).val(v);
							} else {
								$(this).html(v);
							}
						});
					}
				}
			}
		}
	});
})(jQuery);
$.parseUrlArgs = function () {
	var eqIndex = document.URL.lastIndexOf("?");
	var urlParams = document.URL.substring(eqIndex + 1);
	var args = {};
	if (urlParams && urlParams != "") {
		var orgArgs = urlParams.split("&");
		for (var i = 0; i < orgArgs.length; i++) {
			var a = orgArgs[i].split("=");
			args[a[0]] = a[1];
		}
	}
	return args;
};
$.downloadFile=function(fileName, downloadName){
	$.get(saas.interfaces.downloadAttachment+"/"+fileName+"/"+downloadName)
};
Array.prototype.where = function (func) {
	var result = [];
	for (var i = 0; i < this.length; i++) {
		if (func(this[i], i)) result.push(this[i]);
	}
	return result;
};
Array.prototype.select = function (func) {
	var result = [];
	for (var i = 0; i < this.length; i++) result.push(func(this[i]));
	return result;
};
Array.prototype.skip = function (n) {
	var result = [];
	for (var i = n; i < this.length; i++) result.push(this[i]);
	return result;
};
Array.prototype.take = function (n) {
	var result = [];
	for (var i = 0; i < Math.min(n, this.length) ; i++) result.push(this[i]);
	return result;
};
Array.prototype.any = function (func) {
	for (var i = 0; i < this.length; i++) {
		if (func(this[i])) return true;
	}
	return false;
};

//bussiness interfaces
saas={
	getFormData: function (pEle) {
	    var data = {};
	    $.each(pEle.serializeArray(), function () {
	        data[this.name] = this.value;
	    });
	    return data;
	},
	post: function(url,data,okfun,badfun,sender){
		var panel=null;
		if(sender) panel=showLoading(sender);
		$.post(url,data,function(data,status,xhr){
			if(panel) panel.remove();
			if(data.state==1){
				var extra=data.extra;
				if(extra){
					curRole = extra.roles;
					curUserId = extra.userId;
				}

				var pageInfo = data.pageInfo;
				data = data.data || data;
				if(okfun){
					okfun(data, data.pageCount,data.recordCount,data.extra, pageInfo);
				}else{
					saas.alert("操作成功");
				}
			}else{
				if(badfun){
					badfun("本次请求失败: "+data.msg);
				}else{
					saas.httpAlert({
						url:url,
						requestParam:data,
						xhr:xhr
					})
				}
			}
		},"json").fail(badfun||function(xhr,status,msg){
			saas.httpAlert({
				url:url,
				requestParam:data,
				xhr:xhr
			});
			if(panel) panel.remove();
		}).done(function(xhr){
			if(panel) panel.remove();
		});
	},
	// bootstrapIconName: 是 bootstrap3 的图标名称, 如 glyphicon-search
	alert:function(msg,title,timeout,width,bootstrapIconName){
		var mask=$("<div class='saasalertbox'  style='position:absolute;top:0px;left:0px;width:100%;height:100%;background-color:rgba(0,0,0,0.4);z-index:19892015'></div>");
		var box=$("<div style='position:absolute;left:50%;top:50%;transform:translate(-50%,-50%);background-color: white;padding: 8px;border-radius:5px;box-shadow: 8px 8px 16px;'></div>");
		box.width(width||400);
		var titleBar=$("<div style='border-bottom: 1px solid lightgray;padding-bottom: 4px;font-weight: bold;'>"+(title||"系统消息")+"</div>");
		var bodyBlock=$("<div style='padding: 8px;'></div>");
		var iconBlock=$("<div style='display:inline-block;'><i class='glyphicon "+(bootstrapIconName||"glyphicon-exclamation-sign")+"' style='font-size: 36px;'></i></div>");
		var contentBlock=$("<div style='display:inline-block;vertical-align: top; padding-left: 16px;position: relative;left: 10px;width: 88%;padding-bottom: 8px;'>"+msg+"</div>");
		bodyBlock.append(iconBlock);
		bodyBlock.append(contentBlock);
		var footerBlock=$("<div style=\"text-align:center;display:block;padding-bottom: 8px;\"><button style=\"width:100px\" type=\"button\" onclick=\"$(this).closest('.saasalertbox').remove()\" class=\"btn btn-primary\">确定</button></div>");
		
		box.append(titleBar);
		box.append(bodyBlock);
		box.append(footerBlock);

		mask.append(box);
		mask.css("top",$(document.body)[0].scrollTop);
		
		$(document.body).append(mask);
		this.close=function(){
			mask.remove();
		};
		this.content=contentBlock;
		/*timeout=parseInt(timeout);
		if(!isNaN(timeout) && timeout>=1000){
			setTimeout(function(){mask.remove();},timeout)
		}*/
		return this;
	},
	alert2:function(msg,title, okFun, noFun, width,bootstrapIconName){
		var mask=$("<div class='saasalertbox'  style='position:absolute;top:0px;left:0px;width:100%;height:100%;background-color:rgba(0,0,0,0.4);z-index:19892015'></div>");
		var box=$("<div style='position:absolute;left:50%;top:50%;transform:translate(-50%,-50%);background-color: white;padding: 8px;border-radius:5px;box-shadow: 8px 8px 16px;'></div>");
		box.width(width||400);
		var titleBar=$("<div style='border-bottom: 1px solid lightgray;padding-bottom: 4px;font-weight: bold;'>"+(title||"系统消息")+"</div>");
		var bodyBlock=$("<div style='padding: 8px;'></div>");
		var iconBlock=$("<div style='display:inline-block;'><i class='glyphicon "+(bootstrapIconName||"glyphicon-exclamation-sign")+"' style='font-size: 36px;'></i></div>");
		var contentBlock=$("<div style='display:inline-block;vertical-align: top; padding-left: 16px;position: relative;left: 10px;width: 88%;padding-bottom: 8px;'>"+msg+"</div>");
		bodyBlock.append(iconBlock);
		bodyBlock.append(contentBlock);
		var footerBlock=$("<div style=\"text-align:center;display:block;padding-bottom: 8px;\"></div>");
		
		var okBtn=$("<button style=\"width:80px;\" type=\"button\"  class=\"btn btn-primary\">确定</button>");
		
		footerBlock.append(okBtn);
		
		okBtn.on("click",function(){
			if(okFun){
				try{
					okFun();
				}catch(x){
					
				}
				mask.remove();
			}
		});
		
		box.append(titleBar);
		box.append(bodyBlock);
		box.append(footerBlock);

		mask.append(box);
		
		$(document.body).append(mask);
	},
	prompt:function(msg,title, okFun,width,bootstrapIconName){
		var mask=$("<div class='saasalertbox'  style='position:absolute;top:0px;left:0px;width:100%;height:100%;background-color:rgba(0,0,0,0.4);z-index:19892015'></div>");
		var box=$("<div style='position:absolute;left:50%;top:50%;transform:translate(-50%,-50%);background-color: white;padding: 8px;border-radius:5px;box-shadow: 8px 8px 16px;'></div>");
		box.width(width||400);
		var titleBar=$("<div style='border-bottom: 1px solid lightgray;padding-bottom: 4px;font-weight: bold;'>"+(title||"系统消息")+"</div>");
		var bodyBlock=$("<div style='padding: 8px;'></div>");
		var iconBlock=$("<div style='display:inline-block;'><i class='glyphicon "+(bootstrapIconName||"glyphicon-exclamation-sign")+"' style='font-size: 36px;'></i></div>");
		var contentBlock=$("<div style='display:inline-block;vertical-align: top; padding-left: 16px;position: relative;left: 10px;width: 88%;'>"+msg+"</div>");
		var inputBlock =$("<div style='text-align:center;padding: 8px;'></div>");
		var input =$("<input type='text' style='height:24px;width:80%;'>");
		inputBlock.append(input);
		//bodyBlock.append(iconBlock);
		bodyBlock.append(contentBlock);
		bodyBlock.append(inputBlock);
		var footerBlock=$("<div style=\"text-align:center;display:block;padding-bottom: 8px;\"></div>");
		
		var okBtn=$("<button style=\"width:80px;margin: 0px 16px;\" type=\"button\"  class=\"btn btn-primary\">确定</button>");
		var cancelBtn=$("<button style=\"width:80px\" type=\"button\" class=\"btn btn-primary\">取消</button>");
		
		footerBlock.append(okBtn);
		footerBlock.append(cancelBtn);
		
		
		okBtn.on("click",function(){
			var result=true;
			if(okFun){	
				try{
					result = okFun($(this).closest("div").parent().find("input[type=text]").val());
				}catch(x){
					
				}
			}
			if(result) mask.remove();
		});
		cancelBtn.on("click",function(){
			mask.remove();
		});
		
		box.append(titleBar);
		box.append(bodyBlock);
		box.append(footerBlock);

		mask.append(box);
		$(document.body).append(mask);
		setTimeout(function(){
			input.focus();			
		}, 3000);
	},
	confirm:function(msg,title, okFun, noFun, timeout,width,bootstrapIconName){
		var mask=$("<div class='saasalertbox' style='position:absolute;top:0px;left:0px;width:100%;height:100%;background-color:rgba(0,0,0,0.4);z-index:19892015'></div>");
		var box=$("<div style='position:absolute;left:50%;top:50%;transform:translate(-50%,-50%);background-color: white;padding: 8px;border-radius:5px;box-shadow: 8px 8px 16px;'></div>");
		box.width(width||400);
		var titleBar=$("<div style='border-bottom: 1px solid lightgray;padding-bottom: 4px;font-weight: bold;'>"+(title||"系统消息")+"</div>");
		var bodyBlock=$("<div style='padding: 8px;'></div>");
		var iconBlock=$("<div style='display:inline-block;'><i class='glyphicon "+(bootstrapIconName||"glyphicon-exclamation-sign")+"' style='font-size: 36px;'></i></div>");
		var contentBlock=$("<div style='display:inline-block;vertical-align: top; padding-left: 16px;position: relative;left: 10px;width: 88%;padding-bottom: 8px;'>"+msg+"</div>");
		bodyBlock.append(iconBlock);
		bodyBlock.append(contentBlock);
		var footerBlock=$("<div style=\"text-align:center;display:block;padding-bottom: 8px;\"></div>");
		
		var okBtn=$("<button style=\"width:80px;margin: 0px 16px;\" type=\"button\"  class=\"btn btn-danger\">是</button>");
		var cancelBtn=$("<button style=\"width:80px\" type=\"button\" class=\"btn btn-default\">否</button>");
		
		footerBlock.append(okBtn);
		footerBlock.append(cancelBtn);
		
		okBtn.on("click",function(){
			if(okFun){
				try{
					okFun();
				}catch(x){
					
				}
				mask.remove();
			}
		});
		cancelBtn.on("click",function(){
			if(okFun){
				try{
					noFun();
				}catch(x){
					
				}
				mask.remove();
			}
		});
		cancelBtn.on("click",function(){
			mask.remove();
		});
		
		box.append(titleBar);
		box.append(bodyBlock);
		box.append(footerBlock);

		mask.append(box);
		
		$(document.body).append(mask);
	},
	httpAlert:function(opts){ // opts: {url:string,requestParam:object,xhr:xhr}
		var msg="抱歉,本次获取数据失败.";
		var saasAlert=saas.alert(msg,"错误");
		var devBlock=$("<div style='margin-top:10px;'></div>");
		var devTitle=$("<div style='cursor:pointer'><i class='glyphicon glyphicon-triangle-right'></i> 开发人员信息</div>");
		devTitle.on("click",function(){
			event.preventDefault();
			event.stopPropagation();
			var next=$(this).next();
			var icon=$(this).children("i");
			next.toggle();
			if(next.is(":hidden")){
				icon.removeClass("glyphicon-triangle-bottom");
				icon.addClass("glyphicon-triangle-right");
			}else{
				icon.removeClass("glyphicon-triangle-right");
				icon.addClass("glyphicon-triangle-bottom");
			}
		});
		var bodyMessage=opts.xhr.responseText||"";
		if(bodyMessage!=""){
			var x=/<h1*>(.*?)<\/h1>/g.exec(bodyMessage);
			if(x && x.length>1) bodyMessage=x[1];
		}
		var content="<b>请求 url:</b>"+opts.url +"<br/><b>请求参数:</b>"+JSON.stringify(opts.requestParam)+"<br/><b>http响应值:</b>"+opts.xhr.status+"<br/><b>http信息:</b>"+opts.xhr.statusText+"<br/><b>http响应报文:</b>"+bodyMessage;
		var devContent=$("<div style='display:none;'>"+content+"</div>");
		devBlock.append(devTitle);
		devBlock.append(devContent);
		saasAlert.content.append(devBlock);
	}
}

function showLoading(forEle){
	var bound=forEle[0].getBoundingClientRect();
	var loadingPanel=$("<div class='loadingPanel'></div>");
	$(document.body).append(loadingPanel);
	loadingPanel.css({
		left:bound.left,
		top:bound.top,
		width:bound.right-bound.left,
		height:bound.bottom-bound.top
	});
	loadingPanel.show();
	return loadingPanel;
}

Date.prototype.DateAdd = function(strInterval, Number) {     
    var dtTmp = this;    
    switch (strInterval) {     
        case 's' :return new Date(Date.parse(dtTmp) + (1000 * Number));    
        case 'n' :return new Date(Date.parse(dtTmp) + (60000 * Number));    
        case 'h' :return new Date(Date.parse(dtTmp) + (3600000 * Number));    
        case 'd' :return new Date(Date.parse(dtTmp) + (86400000 * Number));    
        case 'w' :return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));    
        case 'q' :return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number*3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());    
        case 'm' :return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());    
        case 'y' :return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());    
    }    
}
Date.prototype.Format = function(formatStr)     
{     
    var str = formatStr;     
    var Week = ['日','一','二','三','四','五','六'];    
    
    str=str.replace(/yyyy|YYYY/,this.getFullYear());     
    str=str.replace(/yy|YY/,(this.getYear() % 100)>9?(this.getYear() % 100).toString():'0' + (this.getYear() % 100));     
    
    str=str.replace(/MM/,this.getMonth()>9? (this.getMonth() +1).toString():'0' + (this.getMonth() + 1));     
    str=str.replace(/M/g,this.getMonth());     
    
    str=str.replace(/w|W/g,Week[this.getDay()]);     
    
    str=str.replace(/dd|DD/,this.getDate()>9?this.getDate().toString():'0' + this.getDate());     
    str=str.replace(/d|D/g,this.getDate());     
    
    str=str.replace(/hh|HH/,this.getHours()>9?this.getHours().toString():'0' + this.getHours());     
    str=str.replace(/h|H/g,this.getHours());     
    str=str.replace(/mm/,this.getMinutes()>9?this.getMinutes().toString():'0' + this.getMinutes());     
    str=str.replace(/m/g,this.getMinutes());     
    
    str=str.replace(/ss|SS/,this.getSeconds()>9?this.getSeconds().toString():'0' + this.getSeconds());     
    str=str.replace(/s|S/g,this.getSeconds());     
    
    return str;     
}

$.autoTable=function(json, tableId, queryForm, pageInfo){
	// 1. 将 json 放入
	var table=$("#"+tableId); 
	var tbody=table.find("tbody");
	if(tbody.length==0){
		tbody=$("<tbody></tbody>");
		table.append(tbody);
	}
	tbody.html("");
	var ths=$("#"+tableId+" tr:first-child th");
	$.each(json,function(){
		var tr=$("<tr></tr>");
		var that=this;
		tr.attr("json",encodeURI(JSON.stringify(this)));
		$.each(ths, function(){
			var p=$(this);
			var td=$("<td></td>");
            var isHidden=p.data("visible")=="hidden";
            if(isHidden){
                p.hide();
                td.hide();
            }
			var v=that[p.attr("data-column-id")];
			td.attr("value",v);
			if(p.data("function")){
				v=window[p.data("function")](that,p.attr("data-column-id"));
			}else{
				var formatter=p.data("formatter");
				if(formatter=="checkbox"){
					v="<input type=\"checkbox\" value=\""+v+"\" />";
					// also, place checkbox in th
					p.html("<input type=\"checkbox\" onclick=\"gridCheckAll(this);\"/>")
				}
				if(formatter=="link"){
					var funmatter=p.data("funmatter");
					if(typeof(v)!="undefined"){
						v="<a href=\"javascript:void(0)\"  onclick=\""+funmatter+"('"+v+"', this);\">"+v+"</a>";	
					}
				}
				if(formatter=="unixtimestamp"){
                    var fmt = $(this).data("modelformat"); // 自定义渲染格式
                    if (fmt) {
						var timestamp=parseInt(v);
						if(!isNaN(timestamp)) {
							v = new Date(parseInt(timestamp)).Format(fmt);
						}
                    }
				}
			}
			td.html(v);
			tr.append(td);
		});
		tbody.append(tr);
	});
	var pagePanel=$("."+tableId + " pagination");
	if(pagePanel.length==0){
		pagePanel=$("<div class='"+tableId+" pagination'></div>");
		table.after(pagePanel);
	}
	pagePanel.data("form",queryForm);
	pagePanel.data("table",tableId);
	if((pageInfo || "").trim() != ""){
	    pagePanel.html('<div class="page-box">' + $("<p></p>").html(pageInfo).text() + '</div>');
	}else{
	    $(".pagination").hide();
    }
	/*table.bootgrid({
		templates:{
			header:"",
			footer:"",
		},
		labels: gridLabelSettings,
		formatters:{
			checkbox:checkboxFormatter,
			unicode:unicodeFormatter,
			link:linkFormatter,
			unixTime:unixTimeFormatter
		}
	});*/
	var addKey="addFor"+tableId;
	if(window[addKey]){
		var addBtn=$("."+addKey);
		addBtn.data("table",tableId);
		if(addBtn.length>0){
			addBtn.on("click",function(){
				var el=$(event.target||event.srcElement);
				var k=el.data("table");
				var view=window["addFor"+k]();
				//var rootDir=$('#rootDirPage')?$('#rootDirPage').val():"";
				var addViewName=window.addViewName||(rootDir+"/wzxt/add/"+view+"/null");
				var frame=$("<iframe></iframe>");
				frame.attr("src",addViewName);
                showDlg("新增",frame, 800,580,function(dlg,content){
                	if(content[0].contentWindow.validateForm!=null){
                		content[0].contentWindow.validateForm(function(ret){
                			if(ret){
                				content[0].contentWindow.$("form").submit();
                			}else
                				{
                				saas.alert('该记录已存在');
                				}
                		});
                	}                	
                },null,$(".dlgFrame"));
			});
		}
	}
	var updateKey="updateFor"+tableId;
	if(window[updateKey]){
		var updateBtn=$("."+updateKey);
		updateBtn.data("table",tableId);
		if(updateBtn.length>0){
			updateBtn.on("click",function(){
				var item={};
				var tid=$(this).data("table");
				try{
					var checkedRows=gridGetChecked(tid);
					if(checkedRows.length==0){
						saas.alert("请选择要修改的数据");
						return;
					}
					if(checkedRows.length>1){
						saas.alert("不能同时修改两条记录");
						return;
					}
					item=checkedRows[0];
				}catch(e){}
				var view=window["updateFor"+tid]();
				var frame=$("<iframe></iframe>");
                showDlg("修改",frame, 800,580,function(dlg,content){
                	content[0].contentWindow.$("form").submit();
                },null,$(".dlgFrame"),function(){
                	var rootDir=$('#rootDirPage')?$('#rootDirPage').val():"";
    				var src=rootDir+"/wzxt/add/"+view+"/"+JSON.stringify(item);
    				frame.attr("src",src);
                });
			});
		}
	}
	var deleteKey="deleteFor"+tableId;
	if(window[deleteKey]){
		var delBtn=$("."+deleteKey);
		if(delBtn.length>0){
			delBtn.data("table",tableId);
			delBtn.on("click",function(){
				var items=[];
				try{
					var tid=$(this).data("table");
					var chks=$("#"+tid).find("tbody tr td:first-child input[type=checkbox]");
					items=gridGetChecked(tid);
					if(items.length==0){
						saas.alert("请选择要删除的数据");
						return;
					}
				}catch(e){}
				window["deleteFor"+tid](items);
			});
		}
	}
};
function autoSubmit(sender, pageNo,pageSize,orderBy){
	if(!pageNo) pageNo=1;
	if(!pageSize) pageSize=10;
	if(!orderBy) orderBy="";
	var pForm=$(sender);
	pForm.find("input[name=pageNo]").val(pageNo);
	pForm.find("input[name=pageSize]").val(pageSize);
	pForm.find("input[name=orderBy]").val(orderBy);
	return true;
}
function page(pageIndex, pageSize, arg){
	var target=event.target||event.srcElement;
	var pagePanel=$(target).closest(".pagination");
	var tableId=pagePanel.data("table");
	var formId=pagePanel.data("form");
	var pTable=$("#"+tableId);
	var pForm=$("#"+formId);
	pForm.find("input[name=pageNo]").val(pageIndex);
	pForm.find("input[name=pageSize]").val(pageSize);
	pForm.submit();
}

function gridCheckAll(sender){
	$(sender).closest("table").find("td:first-child input[type=checkbox]").prop("checked",$(sender).prop("checked"));
}
function gridGetChecked(tableId){
	var trs=$("#"+tableId+" tbody td:first-child input[type=checkbox]:checked").closest("tr");
	if(trs.length==0) return [];
	var table=$("#"+tableId);
	var ths=$("#"+tableId+" th");
	var objs=[];
	$.each(trs,function(){
		var tds=$(this).find("td");
		var obj={};
		$.each(tds,function(idx){
			var v=$(this).attr("value");
			var field=$(ths[idx]).attr("data-column-id");
			obj[field]=v;
		});
		objs.push(obj);
	});
	return objs;
}

function getTRJson(sender){
    var tr=$(sender).closest("tr");
    var data=tr.attr("json");
    var item=JSON.parse(decodeURI(data));
    return item;
}

function openDialog2(dialog,title,width,heith, onCancel){ // returns layer's index
    return layer.open({
        type: 1,
        area: [width+'px', heith+'px'], //宽高
        title:title,
        shade:0,
        //btn:['确定','取消'],
        content:  $('#'+dialog+''), //显示id的内容,
		cancel: onCancel
    });
}

function micformat2date(val) {
    if (val) {
        //IE 不是识别 $1 这种类型正则
        //var date = new Date(eval(val.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")));  
        var date = new Date(parseInt(val.substring(6)));
        return date;
    }
}

$.parseUrlArgs = function () {
    var eqIndex = document.URL.lastIndexOf("?");
    var urlParams = document.URL.substring(eqIndex + 1);
    var args = {};
    if (urlParams && urlParams != "") {
        var orgArgs = urlParams.split("&");
        for (var i = 0; i < orgArgs.length; i++) {
            var a = orgArgs[i].split("=");
            args[a[0]] = a[1];
        }
    }
    return args;
};

function getExploreType() {
    var sys = {}, browserInfo = {},
        ua = navigator.userAgent.toLowerCase(),
        s;
    (s = ua.match(/rv:([\d.]+)\) like gecko/)) ? sys.ie = s[1] :
        (s = ua.match(/msie ([\d\.]+)/)) ? sys.ie = s[1] :
        (s = ua.match(/edge\/([\d\.]+)/)) ? sys.edge = s[1] :
        (s = ua.match(/firefox\/([\d\.]+)/)) ? sys.firefox = s[1] :
        (s = ua.match(/(?:opera|opr).([\d\.]+)/)) ? sys.opera = s[1] :
        (s = ua.match(/chrome\/([\d\.]+)/)) ? sys.chrome = s[1] :
        (s = ua.match(/version\/([\d\.]+).*safari/)) ? sys.safari = s[1] : 0;
    // 根据关系进行判断
    if (sys.ie) return browserInfo = { brand: 'IE', version: sys.ie };
    if (sys.edge) return browserInfo = { brand: 'Edge', version: sys.edge };
    if (sys.fireFox) return browserInfo = { brand: 'Firefox', version: sys.firefox };
    if (sys.chrome) return browserInfo = { brand: 'Chrome', version: sys.chrome };
    if (sys.safari) return browserInfo = { brand: 'Safari', version: sys.safari };
    return browserInfo = { brand: 'Unkonwn' };
}

///格式化金额
function moneyFormat(val) {
    //兼容性不好
    //return (+parseFloat(val).toFixed(2)).toLocaleString();
    var str = (+val).toFixed(2);
    var reverse = function reverse(str) {
        return str.split('').reverse().join('');
    };
    return reverse(reverse(str).replace(/\d{3}/g, '$&,').replace(/\,$/, ''));
}

function isvaildMail(mail) {
    var reg = /^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
    return reg.test(mail);
}

function isvaildPhone(phone) {
    var reg = /^1([358][0-9]|4[579]|66|7[0135678]|9[89])[0-9]{8}$/;
    return reg.test(phone);
}

/// easyui format mircosoft time
function formatDate(val, row, index) {
    var dateStr = '';
    if (val) {
        if (val.indexOf('Date') > 0) {
            var date = new Date(parseInt(val.substring(6)));
            dateStr += date.getFullYear() + '-';
            dateStr += date.getMonth() >= 9 ? (date.getMonth() + 1).toString() + '-' : '0' + (date.getMonth() + 1) + '-'
            dateStr += date.getDate() > 9 ? date.getDate().toString() + ' ' : '0' + date.getDate() + ' ';
            dateStr += date.getHours() > 9 ? date.getHours().toString() + ':' : '0' + date.getHours() + ':';
            dateStr += date.getMinutes() > 9 ? date.getMinutes().toString() + ':' : '0' + date.getMinutes() + ':';
            dateStr += date.getSeconds() > 9 ? date.getSeconds().toString() : '0' + date.getSeconds();
        } else {
            var date = new Date(val);
            dateStr += date.getFullYear() + '-';
            dateStr += date.getMonth() >= 9 ? (date.getMonth() + 1).toString() + '-' : '0' + (date.getMonth() + 1) + '-'
            dateStr += date.getDate() > 9 ? date.getDate().toString() + ' ' : '0' + date.getDate() + ' ';
            dateStr += date.getHours() > 9 ? date.getHours().toString() + ':' : '0' + date.getHours() + ':';
            dateStr += date.getMinutes() > 9 ? date.getMinutes().toString() + ':' : '0' + date.getMinutes() + ':';
            dateStr += date.getSeconds() > 9 ? date.getSeconds().toString() : '0' + date.getSeconds();
        }
    }
    return dateStr;
}