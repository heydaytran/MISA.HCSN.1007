<template>
  <div>
    <div v-show="isActive" class="modal">
      <div class="modal-background"></div>
      <div class="modal-content">
        <div class="header">
          <div v-if="formMode == 'insert'" class="title">
            Thêm chứng từ ghi tăng
          </div>
          <div v-else class="title">Sửa chứng từ ghi tăng</div>

          <div class="header-right">
            <div class="icon-help btn btn-help" title="Hỗ trợ"></div>
            <div
              class="icon-cancel btn btn-close"
              title="Đóng"
              @click="hide()"
            ></div>
          </div>
        </div>

        <div class="content">
          <div class="modal-container input-container container-top">
            <div class="top-label">
              <label for="">Thông tin chứng từ </label>
            </div>
            <div class="container-box input-box">
              <div class="input-row">
                <div class="input-field asset-code">
                  <label for=""
                    >Mã chứng từ <span style="color: red">(*)</span></label
                  >
                  <input
                  placeholder="Nhập mã chứng từ"
                    onClick="this.select();"
                    id="assetInput1"
                    type="text"
                    maxlength="20"
                    class="input-one-third"
                    v-model="assetIncrease.exhibitCode"
                    @keyup="validateAssetCode()"
                    @blur="validateAssetCode()"
                  />
                </div>
                <div class="input-field date-proc">
                  <label for=""
                    >Ngày chứng từ <span style="color: red">(*)</span></label
                  >
                  <DatePicker
                    class="input-sm"
                    input-class="datetime"
                    placeholder="dd/MM/yyyy"
                    v-model="assetIncrease.exhibitDate"
                    type="date"
                    format="DD/MM/YYYY"
                    value-type="YYYY-MM-DD"
                    :disabled-date="disabledAfterToday"
                    @input-error="showWarning('Ngày sai định dạng', true)"
                  >
                    <template v-slot:header="{ emit }">
                      <button
                        class="mx-btn mx-btn-text"
                        @click="emit(new Date())"
                      >
                        Today
                      </button>
                    </template>
                  </DatePicker>
                </div>
                <div class="input-field date-increase">
                  <label for=""
                    >Ngày ghi tăng <span style="color: red">(*)</span></label
                  >
                  <DatePicker
                    class="input-sm"
                    input-class="datetime"
                    placeholder="dd/MM/yyyy"
                    v-model="assetIncrease.increaseDate"
                    type="date"
                    format="DD/MM/YYYY"
                    value-type="YYYY-MM-DD"
                    :lang="lang"
                    :disabled-date="disabledAfterToday"
                    @input-error="showWarning('Ngày sai định dạng', true)"
                  >
                    <template v-slot:header="{ emit }">
                      <button
                        class="mx-btn mx-btn-text"
                        @click="emit(new Date())"
                      >
                        Today
                      </button>
                    </template>
                  </DatePicker>
                </div>
              </div>
              <div class="input-row">
                <div class="input-field field-note">
                  <label for=""
                    >Ghi chú</label
                  >
                  <input
                  placeholder="Nhập ghi chú"
                    onClick="this.select();"
                    id="assetInput1"
                    type="text"
                    maxlength="20"
                    class="input-one-third"
                    v-model="assetIncrease.note"
                    @keyup="validateAssetCode()"
                    @blur="validateAssetCode()"
                  />
                </div>
              </div>
            </div>

            <div id="assetInput1_warning" class="validate-warning">
              Thông tin bắt buộc nhập
            </div>
            <div
              v-show="dup"
              style="display: block"
              id="assetInput1_warning"
              class="validate-warning"
            >
              Mã tài sản bị trùng
            </div>
          </div>

          <div class="modal-container container-below">
            <div class="below-label">
              <label for="">Thông tin tài sản ghi tăng </label>
            </div>
            <div class="container-box grid-box">
              <div class="panel-header">
                <div class="features-pane">
                  <div class="features-pane-left">
                    <input
                      id="assetSearchBox"
                      class="input-search"
                      type="text"
                      placeholder="Tìm kiếm. "
                      @change="
                        getAsset(
                          'filter',
                          comboxFilter.idDepartment,
                          comboxFilter.idType
                        )
                      "
                    />
                    <div class="icon-search"></div>
                  </div>

                  <div class="features-pane-right">
                   

                    <div
                      id="add-asset"
                      class="btn  features-pane-item"
                      @click="showDialogAsset()"
                    >
                    <div class="icon-add">
                        <div class="icon">

                        </div>
                    </div>
                    <div class="text-add">
                        <div class="text">Chọn tài sản</div>
                    </div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="content-grid grid">
                <table class="table-asset" id="tableAsset">
                  <colgroup>
                    <col width="50" />
                    <!-- <col width="120" /> -->
                    <col width="120" />
                    <col min-width="800" />
                    <col min-width="200" />
                    <col min-width="500" />
                    <col width="150" />
                    <col width="100" />
                  </colgroup>
                  <thead>
                    <tr>
                      
                      <th style="text-align: left">STT</th>

                      <th
                        sortProp="code"
                        sortOrder="asc"
                        id="columnAssetCode"
                        class="hover-pointer"
                        style="text-align: left"
                      >
                        Mã tài sản
                      </th>
                      <th
                        sortProp="code"
                        sortOrder="asc"
                        id="columnAssetCode"
                        class="hover-pointer"
                        style="text-align: left"
                      >
                        Tên tài sản
                      </th>
                      <th
                        sortProp="code"
                        sortOrder="asc"
                        id="columnAssetCode"
                        class="hover-pointer"
                        style="text-align: left"
                      >
                        Bộ phận sử dụng
                      </th>
                      <th
                        sortProp="name"
                        sortOrder="asc"
                        id="columnAssetName"
                        class="hover-pointer"
                        style="text-align: left"
                      >
                        Ngyên giá
                      </th>
                      <th
                        sortProp="department"
                        sortOrder="asc"
                        id="columnDepartment"
                        class="hover-pointer"
                        style="text-align: left"
                      >
                        HM/KM lũy kế 
                      </th>
                      <th
                        sortProp="department"
                        sortOrder="asc"
                        id="columnDepartment"
                        class="hover-pointer"
                        style="text-align: left"
                      >
                        Giá trị còn lại 
                      </th>
                         <th
                        sortProp="department"
                        sortOrder="asc"
                        id="columnDepartment"
                        class="hover-pointer"
                        style="text-align: left"
                      >
                        
                      </th>

                    </tr>
                  </thead>

                  <tbody>
                    <tr>
                      
                      <td>hello</td>
                      <td>hello</td>
                      <td>hello</td>
                      <td>hello</td>
                      <td>hello</td>
                      <td>hello</td>
                      <td>hello</td>
                      <td><div data-v-bdaea12c="" class="btn btn-confirm-delete1">Xóa</div></td>
                    </tr>
                   
                  </tbody>
                  <!-- <BaseLoading ref="BaseLoading_ref" /> -->

                  <!-- <div v-show="getEmty" class="loading-emty">
                    Không có dữ liệu
                  </div> -->
                </table>
              </div>
              <div class="footer-grid">
                  <div class="footer-content">
                      Tổng số: 0 tài sản
                  </div>
              </div>
            </div>

            <div id="assetInput1_warning" class="validate-warning">
              Thông tin bắt buộc nhập
            </div>
            <div
              v-show="dup"
              style="display: block"
              id="assetInput1_warning"
              class="validate-warning"
            >
              Mã tài sản bị trùng
            </div>
          </div>
        </div>
        <div class="footer">
          <div class="btn btn-cancel" tabindex="0" @click="hide()">Hủy</div>

          <div class="btn btn-save" tabindex="0" @click="save()">Lưu</div>
        </div>
      </div>
      <AssetManagement v-if="isChoose" />
    </div>
  </div>
</template>

<script src="https://unpkg.com/vuejs-datepicker/dist/locale/translations/fr.js"></script>

<script>
import axios from "axios";

import 'vue2-datepicker/locale/vi';

import DatePicker from "vue2-datepicker";
import "vue2-datepicker/index.css";

import AssetManagement from '../../dictionary/AssetManagement.vue'

export default {
  components: {
    DatePicker,
    AssetManagement
  },
  props: {
    listAssetType: Array,
    listDepartment: Array,
    formMode: String,
    assetIdUpdate: String,
  },
  data() {
    return {
      isActive: false,
      showSuccess: true,
      isChoose:false,

      assetIncrease: {
        increaseId: null,
        exhibitCode: null,
        exhibitDate: null,
        increaseDate:null,
        note:null,
        increaseDetail: null,
        createdBy : null,
        modifiedBy: null,
        createdDate: null,
        modifiedDate:null
      },



      dup: false,
      lang: {
        formatLocale: {
          firstDayOfWeek: 1,
        },
        days: ["CN", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7"],
        months: [
          "Tháng 1",
          "Tháng 2",
          "Tháng 3",
          "Tháng 4",
          "Tháng 5",
          "Tháng 6",
          "Tháng 7",
          "Tháng 8",
          "Tháng 9",
          "Tháng 10",
          "Tháng 11",
          "Tháng 12",
        ],
        weekdays: [
          "Chủ nhật",
          "thứ hai",
          "thứ ba",
          "thứ tư",
          "thứ năm",
          "thứ sáu",
          "Thứ Bảy",
        ],
        weekdaysMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
        yearFormat: "YYYY",
        monthsShort: [
          "T01",
          "T02",
          "T03",
          "T04",
          "T05",
          "T06",
          "T07",
          "T08",
          "T09",
          "T10",
          "T11",
          "T12",
        ],
        monthFormat: "TMM",
      },
    };
  },
  methods: {
   
    async show() {
      var res = this;
      this.isActive = true;
      this.dup = false;

      var warning = document.getElementById("assetInput1");

      warning.classList.remove("borderRed");
      warning.classList.remove("hover-validate");

      

      // document.getElementById("assetInput1_warning").style.display = "none"

      // focus vào input đầu tiên( mã tài sản)

      setTimeout(() => {
        document.getElementById("assetInput1").focus();
      }, 0);
      //  document.getElementById("assetInput1").focus();

      document.getElementsByClassName("body-right")[0].style.zIndex = "999";
      if (this.formMode == "update") {
        await axios
          .get("https://localhost:44382/api/v1/Assets/" + this.assetIdUpdate)
          .then((Response) => {
            res.asset.departmentId = "";
            res.asset = Response.data.data;

            // debugger; // eslint-disable-line no-debugger
          })
          .catch((error) => {
            this.errorMessage = error.message;
            console.error("GET Asset by id Failed: ", error.message);
          });

        // convert ngyên giá sang string
        var a = res.asset.originalPrice;
        if (a == null) a = "";
        else a = res.asset.originalPrice.toString();

        res.asset.originalPrice = a;
        //debugger; // eslint-disable-line no-debugger
      } else {
        this.resetInput();
      }
    },

    // todo ẩn dialog
    hide() {
      this.isActive = false;
      document.getElementsByClassName("body-right")[0].style.zIndex = "0";
    },

    // todo select tất cả nội dung ô input khi click
    selectAll() {
      document.getElementsByTagName("input").forEach((element) => {
        element.addEventListener("click", () => {
          element.select();
        });
      });
    },

// todo hiện form chọn tài sản
showDialogAsset()
{
this.isChoose = true
},
    // todo lấy dữ liệu tên phòng ban
    getDepartmentName() {
      var res = this;
      this.listDepartment.forEach((element) => {
        if (element.departmentId == res.asset.departmentId) {
          res.asset.departmentName = element.departmentName;
        }
      });
    },

    //todo lấy dữ liệu tên loại tài sản
    getAssetTypeName() {
      var res = this;
      this.listAssetType.forEach((element) => {
        if (element.assetTypeId == res.asset.assetTypeId) {
          res.asset.assetTypeName = element.assetTypeName;
        }
      });
    },

    // todo chỉ cho phép nhập số
    formatNumber(e) {
      var key = e.key;
      if (!/^\d+/g.test(key)) {
        e.preventDefault();
      }

      // if((this.asset.originalPrice == null ) && this.asset.wearValue == null)
      // {
      //   this.asset.originalPrice = "0";
      //   this.asset.wearValue  = "0"
      // }
      // if( this.asset.originalPrice == '')
      // {
      //   this.asset.originalPrice = "0";
      // }
      // if(this.asset.wearValue == '')
      // {
      //   this.asset.wearValue  = "0"
      // }

      // setTimeout(() => {
      //   if (
      //     parseInt(this.asset.originalPrice) <= parseInt(this.asset.wearValue)
      //   ) {
      //     this.asset.wearValue = null;
      //   }
      // }, 200);
    },

    //todo định dạng kiểu tiền tệ
    formatMoney(money) {
      return money.replace(/\B(?=(\d{3})+(?!\d))/g, `.`);
    },
    /**
     * ẩn hết những ngày sau ngày hiện tại
     * CreatedBy: TVHIEN (13/04/2021)
     */
    disabledAfterToday(date) {
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      return date > today;
    },

    // todo bỏ định dạng tiền tệ
    removeFormatMoney(money) {
      return money.replace(/\D+/g, "");
    },

    //todo chuyển số thành chuỗi
    numberToString(input) {
      if (input == null) return "";
      else return input.toString();
    },

    // todo chuyển chuỗi thành số
    stringToNumber(input) {
      if (input == "" || input == "0") return null;
      else return parseInt(input);
    },

    // update input
    updateInput(text, e) {
      if (text == "wearValue")
        this.asset.wearValue = this.removeFormatMoney(e.target.value);
      else this.asset.originalPrice = this.removeFormatMoney(e.target.value);
    },

    // validate trống trường dữ liệu mã tài sản
    async validateAssetCode() {
      this.dup = false;
      var warning = document.getElementById("assetInput1");
      if (this.asset.assetCode == null || this.asset.assetCode == "") {
        // warning.style.border = "1px solid red";
        warning.classList.add("borderRed");
        warning.classList.add("hover-validate");
      } else {
        warning.style.border = "#e4e4e4 1px solid";
        warning.classList.remove("hover-validate");
        warning.classList.remove("borderRed");
      }
    },

    // todo validate trường dữ liệu tên tài sản
    validateAssetName() {
      var warning = document.getElementById("assetInput2");
      if (this.asset.assetName == null || this.asset.assetName == "") {
        warning.classList.add("borderRed");
        warning.classList.add("hover-validate");
      } else {
        warning.style.border = "#e4e4e4 1px solid";
        warning.classList.remove("hover-validate");
        warning.classList.remove("borderRed");
      }
    },

    // todo lưu dữ liệu
    async save() {
      this.validateAssetName();
      this.validateAssetCode();
      if (
        parseInt(this.asset.originalPrice) <= parseInt(this.asset.wearValue)
      ) {
        document.getElementById("assetInput8").classList.add("borderRed");
        return;
      } else {
        document.getElementById("assetInput8").classList.remove("borderRed");
      }
      var res = this;
      if (res.asset.originalPrice == "") res.asset.originalPrice = null;
      if (res.asset.wearValue == "") res.asset.wearValue = null;

      if (
        this.asset.assetCode == null ||
        this.asset.assetName == null ||
        this.asset.assetCode == "" ||
        this.asset.assetName == ""
      ) {
        return;
      } else {
        if (this.formMode == "insert") {
          //nếu là form thêm dữ liệu
          await axios
            .post("https://localhost:44382/api/v1/assets/", this.asset)
            .then((respone) => {
              // nếu không gặp lỗi badrequest
              if (respone.data.errorCode == 400) {
                var warning = document.getElementById("assetInput1");
                res.dup = true;
                warning.classList.add("borderRed");

                res.$emit("reload", false);
                return;
              } else if (
                respone.data.errorCode != 400 &&
                parseInt(res.asset.originalPrice) <=
                  parseInt(res.asset.wearValue)
              ) {
                res.dup = false;
                return;
              } else {
                res.dup = false;
                res.hide();
                res.$emit("reload", true);
              }
            })
            .catch((error) => {
              res.$emit("reload", false);
              console.log(error);
              alert("Có lỗi xảy ra, vui lòng liên hệ MISA để được trợ giúp");
            });
        } else {
          // nếu là form sửa dữ liệu

          await axios
            .put("https://localhost:44382/api/v1/assets/", this.asset)
            .then((respone) => {
              if (respone.data.errorCode != 400) {
                res.hide();
                res.$emit("reload", true);
              } else {
                res.dup = true;
                document
                  .getElementById("assetInput1")
                  .classList.add("borderRed");

                res.$emit("reload", false);
                return;
              }
            })
            .catch((error) => {
              console.log(error);
              res.$emit("reload", false);
              alert("Có lỗi xảy ra, vui lòng liên hệ MISA để được trợ giúp!");
            });
        }
      }
    },

    showWarning(text) {
      this.$emit("msgAlert", text, true);
    },
    checkPostedDate() {
      this.$emit("reload", false);
    },
  },
  watch: {
    // todo theo dõi id phòng ban để lấy ra tên phòng ban tương ứng
    "asset.departmentId": function () {
      if (this.asset.departmentId == null) this.asset.departmentName = null;
      else this.getDepartmentName();
    },

    // todo theo dõi mã loại tài sản để lấy ra tên loại tài sản tương ứng
    "asset.assetTypeId": function () {
      if (this.asset.assetTypeId == null) this.asset.assetTypeId = null;
      else this.getAssetTypeName();
    },
  },
  computed: {
    // todo nhận được kiểu định dạng tiền tệ khi nguyên giá thay đổi
    formatedMoney() {
      return this.formatMoney(this.numberToString(this.asset.originalPrice));
    },
    formatedWearValue() {
      return this.formatMoney(this.numberToString(this.asset.wearValue));
    },
  },
  filters: {},
  mounted() {
    this.selectAll();
  },
};
</script>





<style lang="scss" scoped>
input {
  padding: 8px 16px;
  outline: none;
  border: #beccc9 1px solid;
}

input.required {
  border: red 1px solid !important;
}

.input-search {
  width: 200px;
  padding: 8px 46px 8px 16px;
  border: 1px solid #beccc9;
}

.input-field .input-black {
  background-color: #f5f5f5;
  pointer-events: none;
}

.input-field .input-two-third {
  width: 437px;
}

.input-field .input-one-third {
  width: 210px;
}

.custom-autocomplete input {
  border: none;
  padding-left: 10px;
  font-size: 14px;
}

.v-text-field > .v-input__control > .v-input__slot:before {
  border-style: none;
}

.v-input {
  padding-top: 0px;
  margin-top: 0px;
}

.v-text-field__details {
  display: none;
}

.v-input__slot {
  margin-bottom: 0px;
}

.v-list-item__content .v-list-item__title {
  font-size: 14px;
}

// ::-webkit-scrollbar {
//   width: 8px;
//   height: 8px;
//  }

.test-autocomplete {
  border: 1px solid black;
}

.modal .validate-warning {
  font-family: GoogleSans-Regular;
  font-size: 13px;
  color: red;
  background-color: white;
  /* z-index: 22; */
  position: absolute;
}

.modal-content {
  position: absolute;
  top: calc(50% - 275px);
  left: calc(50% - 350px);
  width: 705px;
  height: 530px;
  background-color: white;
  resize: both;
  overflow: auto;
  min-height: 530px;
  min-width: 705px;
  height: 530px;
  width: 705px;

  .header {
    width: 100%;
    height: 60px;
    display: flex;
    padding: 0 16px;
    box-sizing: border-box;
    display: flex;
    align-items: center;

    .title {
      line-height: 50px;
      font-family: GoogleSans-Bold;
      font-size: 20px;
    }

    .header-right {
      position: absolute;
      right: 0;
      display: flex;
      align-items: center;
      padding: 0 12px;

      .icon-cancel {
        background-size: 16px;
        margin-left: 2px;
      }
    }
  }

  .content {
    width: 100%;
    height: calc(100% - 110px);
    padding: 16px 0px 16px 16px;
    box-sizing: border-box;
    overflow-y: auto;
    overflow-x: hidden;

    .input-field {
      float: left;
      padding: 0 16px 16px 0px;
      position: relative;
      height: 77px;

      label {
        display: block;
        padding: 2px 2px 4px 2px;
      }

      input {
        height: 34px;
        box-sizing: border-box;
        border: #e4e4e4 1px solid;
        // outline-color: lightgreen;
        padding: 10px;
      }
    }
  }

  .footer {
    width: 100%;
    height: 50px;
    background-color: #f5f5f5;
    display: flex;
    align-items: center;
    position: relative;
    padding: 0 16px;
  }
}

.validate-warning {
  display: none;
  position: absolute;
  font-style: italic;
  padding-right: 0;
}
.hover-validate:hover ~ .validate-warning {
  display: block;
}
.modal-content .header .title {
  font-family: GoogleSans-Bold !important;
}
.borderRed {
  border: 1px solid red !important;
}

#assetInput5 {
  text-align: right;
}
#assetInput6 {
  text-align: right;
}

.modal-content {
  width: 880px;
}
.modal-container {
  width: 100%;
  height: 50%;
}
.modal-content .header {
  border-bottom: 1px solid;
  background: white;
}
.modal-content {
  background: #f5f6fa;
}
.container-box {
  height: 100%;
}

.below-label {
  height: 34px;
}
.top-label {
  height: 34px;
}
.container-box {
  background: white;
}
.container-below {
  box-sizing: border-box;
}
.container-top {
  padding-bottom: 40px;
}
.modal-content .content .input-field input {
  border-radius: 2px;
}
#assetInput1 {
  width: 100%;
}
.input-row .asset-code {
  width: 50%;
}
.date-proc,
.date-increase {
  width: 25%;
}
.panel-header .features-pane{
    display: flex;
}
.features-pane-left{
    position: relative;
}
.icon-search{
    position:absolute;
    left: 0;
    top: 5;
}
.input-search {
    width: 200px;
    padding: 8px 46px 8px 38px;
    border: 1px solid #beccc9;
}
.features-pane-right {
    position: absolute;
    right: 24px;
}
.features-pane-right #add-asset .icon{
    background: red;
}
.features-pane-right #add-asset{
    width: 150px;
}

table  td, th{
white-space: nowrap;
}
.modal-content .header {
    border-bottom: 1px solid #e4e4e4;
    background: white;
}
.modal-content{

    height: 578px;
}
.container-box {
    background: white;
    box-shadow: 0 0 7px #bbb6b6;
    border: 1px solid #e4e4e4;
}
.container-below {
    box-sizing: border-box;
    overflow: hidden;
}
.modal-content .footer {
   
    border-top: 2px solid #eaeaea;
}
.container-box.grid-box {
    height: calc(100% - 34px);
}
.container-box {
    padding: 16px;
}
.modal-content[data-v-654548ea] {
    top: calc((100% - 616px)/2);
    left: calc((100% - 880px)/2);
    height: 616px;
}
.modal-container {
    width: 100%;
    height: 45%;
}
.container-box {
    height: calc(100% - 34px);
}
.container-top{
    /* padding-bottom: 40px; */
     padding-bottom: 0px; 
}
.modal-content .content  .field-note{
    width: 100%;
}
.modal-content .content .input-field{
    padding: 0 0px 16px 0px;
}
.modal-content .content {
    padding: 16px;
}
.input-field.asset-code {
    box-sizing: border-box;
}
.input-field.date-proc {
    box-sizing: border-box;
}
.input-field.date-proc {
    padding-left: 16px!important;
}
.input-field.date-increase {
    padding-left: 16px!important;
}
::placeholder {
  color: rgb(191, 194, 191);
  font-size: 12px;
  
}
.modal-container.container-below {
    height: 55%;
    padding-top: 30px;
}
input#assetSearchBox {
    height: 34px;
    border-radius: 2px;
}
.icon-search {
    position: absolute;
    left: 0;
    top: 3px;
}
input#assetSearchBox {
    height: 34px;
    width: 246px;
    border-radius: 2px;
}
.features-pane-right #add-asset {
    width: 150px;
    border: 1px solid #beccc9;
}
.icon-add {
     height: 20px;
    width: 20px;
    background-image:  url("../../../assets/icon/add-icon.png");
    background-repeat: no-repeat;
    background-size: contain;
}
.features-pane-right #add-asset {
    width: 150px;
    border: 1px solid #beccc9;
    white-space: nowrap;
    display: flex;
    align-items: center;
}
.icon-add[data-v-654548ea] {
    height: 25px;
    width: 25px!important;
    background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAYAAADEtGw7AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAALgSURBVEhLxVXpSypxFH1/dQWBUVaY7WRlpiCF7SX6wSjSXJKUCNQPUuHWRlbaomF753GuM+I4QTwej3e+TN3fzPnde+6511/4R/g/xB8fH3h6ekKxWMTp6SkymQzOzs5wfX2NWq2Gz89P5U09viX++voSwkKhgGg0itXVVczOzmJ6ehp2ux3r6+uIx+M4Pz+XC/h+K3TEfOnh4UEIJycn0dXVhf7+fgwPD8PlcsFsNqOvrw8GgwE2mw2Hh4eoVCo6ch0xM/B6vejp6cHY2BhCoRDK5bJyWsfNzQ22t7cxMDAglwQCATw/PyundeiIfT4fjEYj5ubmkMvlROfWbPg/40dHR7BarTCZTAiHw8ppHRrik5MTjI+Pw+l0yt/v7+8S5/Py8hIrKyu4u7trNO3t7Q2pVAozMzPSg3w+L3FCQ8ym8PZYLIaXlxclCikzmUxKJaxCvZDgGWUZGhqSpwoN8cTEhJSWzWaVSB3UnU3q7OzE8fGxZNoMZm2xWEQ+9dIGcalUkmzX1tbEt/yYpe3t7WF3dxeLi4vo6OjAxsaGNJRxykMi2m5hYQFTU1ONRjeImSVttbm5KToyy/39ffEubUebtbW1YWRkRAgYZ6aUjC5xu91S8cXFhfA1iHnIjD0eD25vb/H6+iqTRittbW1hfn4e7e3t0sCdnR2JM1O1scvLy3IhvyU0GtMRND0b1IyfNE6n0+IMukk90xAzG5r+4OBA8zE7n0gk0N3dLZI1u4KV+f1+DA4OiowqNMQ0PDVUh0P1q9ogjjSbo8Y5JMyWVTLj5ko1xARdwT1Aza6urpRoHa0TyE3ncDikEvahGTrix8dHsVRvb6+4gcPCTdcMLp1gMIjR0VFxEgejWq0qp3XoiJkVPU3duIQ4bSRguewBB4hycUlxKLgF7+/vf95uBDXk6qQDOBzUVvUun0tLS4hEImJHZtpKSnxLrILNYdk0PS9ho/ikbynPH/+C/D2A35wi2nqWTFWmAAAAAElFTkSuQmCC);
    background-repeat: no-repeat;
    /* background-size: contain; */
    background-size: 20px 20px;
    background-position: center;
}
.features-pane-right #add-asset[data-v-654548ea] {
    padding: 0 10px!important;
}
.features-pane-right #add-asset[data-v-654548ea] {
    width: auto;
}
.features-pane-right[data-v-654548ea] {
    position: absolute;
    right: 34px;
}
.modal-content {
    top: calc((100% - 632px)/2)!important;
    left: calc((100% - 880px)/2)!important;
    height: 632px!important;
}
.panel-header {
  padding-bottom: 20px ;
}

th {
    background: #f5f6fa!important;
}
.panel-header[data-v-654548ea] {
    /* padding-bottom: 20px; */
    padding: 16px;
    padding-bottom: 20px;
}
.container-box[data-v-654548ea] {
    padding: 0px;
}
table tr td{
  border-right:none ;
}
.container-box.input-box {
    padding: 16px!important;
}
.panel-header {
    /* padding-bottom: 20px; */
    padding: 16px;
    /* padding-bottom: 20px; */
    padding-bottom: none;
}
.modal-container{
    width: 100%;
    height: 42%;
}
.modal-container.container-below{
    height: 58%;
    padding-top: 30px;
}
.footer-grid {
    display: flex;
    align-items: center;
    padding-top: 11px;
    height: auto;
}
.footer-content {
    height: 34px;
    display: flex;
    align-items: center;
    border-top: 1px solid;
    width: 100%;
}
.modal-container.container-below{
    height: 58%;
    padding-top: 22px;
}
.footer-content {
    height: 34px;
    display: flex;
    align-items: center;
    border-top: 1px solid #d6d6d6;
    width: 100%;
}
.footer-content {
    padding-left: 16px;
}
.btn.btn-cancel {
    position: absolute;
    right: 113px;
}

.btn-confirm-delete1 {
    background-color: #ff4646;
    color: white;
    margin-left: 23px;
    height: 28px;
        box-sizing: border-box;
    padding: 0 13px !important;
    width: 50px;
}
table tr{
  cursor: pointer;
}
table th{
  cursor: default;
}
</style>