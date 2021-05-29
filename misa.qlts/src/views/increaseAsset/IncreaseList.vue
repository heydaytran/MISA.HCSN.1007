<template>
  <div class="div-container content" id="content">
    <div class="div-container">
      <div class="panel-feature-top">
        <div class="panel-feature">
          <div class="feature-left">
            <div class="text-title">
              <div class="text">Ghi tăng tài sản</div>
            </div>
            <div class="button-refresh">
              <div class="hover-pointer icon"></div>
            </div>
          </div>
          <div class="feature-right">
            <div
              class="btn-add-asset btn features-pane-item"
              @click="showDialog('insert', 0)"
            >
              Thêm
            </div>
            <div class="hover-pointer button-option-view" >
              <div class="hover-pointer icon-view"  v-bind:class="{ 'icon-view-all' : viewAll, 'icon-view-detail': !viewAll }"></div>
              <div class="hover-pointer icon-arrow"></div>
              <div class="menu-option-view">
                <div class="item-option option-full"  @click="changeVisual('all')">
                  <div v-if="viewAll" class="hover-pointer icon-chosen"></div>
                  <div class="hover-pointer icon-view-all"></div>
                </div>
                <div class="item-option option-detail" @click="changeVisual('detail')">
                  <div v-if="!viewAll"  class="hover-pointer icon-chosen"></div>
                  <div class="hover-pointer icon-view-detail"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="content-nav">
        <v-alert
          id="success-dialog"
          v-show="showSuccess"
          color="green"
          elevation="30"
          type="success"
        >
          {{ alerMsg }}</v-alert
        >
        <v-alert v-if="showWarning" id="required-choose" type="warning">{{
          alerMsg
        }}</v-alert>

        <!-- <v-alert v-if="showError" id="required-choose" type="warning">{{
          alerMsg
        }}</v-alert> -->

        <div v-show="isFilterMenu" class="filter-menu">
          <div class="text" @click="sendOption('Tất cả')">Tất cả</div>
          <div class="item-filter item-type">
            <div class="text" @click="sendOption('Loại tài sản')">
              Loại tài sản
            </div>
            <div class="menu-container menu-type">
              <div class="menu-child list-type">
                <div
                  class="item"
                  v-for="(item, index) in listAssetType"
                  :key="index"
                  @click="getAsset('filter', '', item.assetTypeId)"
                >
                  {{ item.assetTypeName }}
                </div>
              </div>
            </div>
          </div>

          <div class="item-filter item-department">
            <div class="text" @click="sendOption('Phòng ban')">Phòng ban</div>
            <div class="menu-container menu-department">
              <div class="menu-child list-department">
                <div
                  class="item"
                  v-for="(item, index) in listDepartment"
                  :key="index"
                  @click="getAsset('filter', item.departmentId, '')"
                >
                  {{ item.departmentName }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="features-pane">
          <div class="features-pane-left">
            <input
              id="assetSearchBox"
              class="input-search"
              type="text"
              placeholder="Tìm kiếm. "
              v-model="inputSearch"
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
            <div class="button-print">
              <div class="icon"></div>
            </div>
           
            <div
              id="preventLeftClick"
              class="btn icon-trash features-pane-item"
              @click="showDeleteDialog()"
            ></div>
             <div
              class="btn icon-print features-pane-item"
            ></div>
             <div
              class="btn icon-more features-pane-item"
            ></div>
          </div>
        </div>

        <div class="clear-float"></div>
      </div>

      <div id="loadBar"></div>
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
              <th>
                <div class="checkbox-icon">
                  <input type="checkbox" />
                </div>
              </th>
              <th style="text-align: left">STT</th>

              <th
                sortProp="code"
                sortOrder="asc"
                id="columnAssetCode"
                class="hover-pointer"
                style="text-align: left"
              >
                SỐ CHỨNG TỪ
              </th>
              <th
                sortProp="code"
                sortOrder="asc"
                id="columnAssetCode"
                class="hover-pointer"
                style="text-align: left"
              >
                NGÀY CHỨNG TỪ
              </th>
              <th
                sortProp="code"
                sortOrder="asc"
                id="columnAssetCode"
                class="hover-pointer"
                style="text-align: left"
              >
                NGÀY GHI TĂNG
              </th>
              <th
                sortProp="name"
                sortOrder="asc"
                id="columnAssetName"
                class="hover-pointer"
                style="text-align: left"
              >
                NỘI DUNG
              </th>
              <th
                sortProp="department"
                sortOrder="asc"
                id="columnDepartment"
                class="hover-pointer"
                style="text-align: left"
              >
                TỔNG NGUYÊN GIÁ
              </th>

              <th style="text-align: left">chức năng</th>
            </tr>
          </thead>

          <tbody>
            <!-- <tr
              v-for="(asset, index) in listAsset"
              :key="asset.assetId"
              v-bind:class="selectedRow(asset.assetId) ? 'selected-row' : ''"
              @click="selectRow(asset.assetId, $event)"
              @click.right="showContexMenu(asset.assetId, $event)"
            >
              <td class="no-border-left">{{ index + 1 }}</td>
              <td>{{ asset.assetCode }}</td>
              <td>{{ asset.assetName }}</td>
              <td>{{ asset.assetTypeName }}</td>
              <td>{{ asset.departmentName }}</td>

              <td class="no-border-right">
                <div class="features-box">
                  <div
                    :id="'tableRow' + index + '_edit'"
                    class="table-icon icon-edit-pen"
                    @click="showDialog('update', asset.assetId)"
                    title="Sửa"
                  ></div>
                  <div
                    id="preventLeftClick"
                    class="table-icon icon-trash-table"
                    @click="showDeleteDialog('inRow')"
                    title="Xóa"
                  ></div>
                  <div
                    class="table-icon icon-refresh-time"
                    title="Chức năng chưa phát triển"
                  ></div>
                </div>
              </td>
            </tr> -->
            <tr>
              <td>
                <div class="checkbox-icon">
                  <input type="checkbox" />
                </div>
              </td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td class="no-border-right">
                <div class="features-box">
                  <div
                    :id="'tableRow' + index + '_edit'"
                    class="table-icon icon-edit-pen"
                    @click="showDialog('update', asset.assetId)"
                    title="Sửa"
                  ></div>
                  <div
                    id="preventLeftClick"
                    class="table-icon icon-trash-table"
                    @click="showDeleteDialog('inRow')"
                    title="Xóa"
                  ></div>
                  <div
                    class="table-icon icon-refresh-time"
                    title="Chức năng chưa phát triển"
                  ></div>
                </div>
              </td>
            </tr>
            <tr>
              <td>
                <div class="checkbox-icon">
                  <input type="checkbox" />
                </div>
              </td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td>hello</td>
              <td class="no-border-right">
                <div class="features-box">
                  <div
                    :id="'tableRow' + index + '_edit'"
                    class="table-icon icon-edit-pen"
                    @click="showDialog('update', asset.assetId)"
                    title="Sửa"
                  ></div>
                  <div
                    id="preventLeftClick"
                    class="table-icon icon-trash-table"
                    @click="showDeleteDialog('inRow')"
                    title="Xóa"
                  ></div>
                  <div
                    class="table-icon icon-refresh-time"
                    title="Chức năng chưa phát triển"
                  ></div>
                </div>
              </td>
            </tr>
          </tbody>
          <BaseLoading ref="BaseLoading_ref" />

          <div v-show="getEmty" class="loading-emty">Không có dữ liệu</div>
        </table>
        <div class="ctx-menu" id="ctxMenu">
          <div class="ctx-menu-item" @click="showDialog('insert', 0)">Thêm</div>
          <div
            class="ctx-menu-item"
            @click="showDialog('update', listSelectRow[0])"
          >
            Sửa
          </div>
          <div
            id="preventLeftClick"
            class="ctx-menu-item"
            @click="showDeleteDialog(listSelectRow[0])"
          >
            Xóa
          </div>
        </div>
      </div>

      <div class="table-summary">
        <div class="summary">
          <div class="asset-number">Tổng số tài sản: {{ amountAsset }}</div>

          <div class="paging-toolbar">
            <div class="leftchild">
              <div
                class="p-button first-page"
                @click="paging.pageNumber = 1"
              ></div>
              <div class="p-button prev-page" @click="backPage()"></div>
              <div style="margin: 0 16px 0 4px">Trang</div>
              <input
                type="number"
                class="text-pagebumber"
                v-model="paging.pageNumber"
              />
              <div style="margin: 0 16px 0 6px">
                Trên {{ paging.amountPage }}
              </div>

              <div class="p-button next-page" @click="nextPage()"></div>
              <div
                class="p-button last-page"
                @click="paging.pageNumber = paging.amountPage"
              ></div>
              <div class="p-button refresh" @click="getAsset()"></div>
              <select
                name=""
                id=""
                class="select-quantitypage"
                v-model="paging.recordNumber"
                @change="getAsset('filter')"
              >
                <option value="25">25</option>
                <option value="50">50</option>
                <option value="100">100</option>
              </select>
            </div>
          </div>

          <div class="price-number">
            Tổng nguyên giá: {{ totalPrice | formatMoney() }}
          </div>
        </div>
      </div>

      <div id="assetPopup"></div>
    </div>

    <ModalCreate
      ref="ModalCreateAsset_ref"
      :listDepartment="listDepartment"
      :listAssetType="listAssetType"
      :formMode="formMode"
      :assetIdUpdate="assetIdUpdate"
      @reload="reload"
      @msgAlert="msgAlert"
    />
    <ModalDeleteAsset
      @reload="reload"
      :listSelectRow="listSelectRow"
      ref="ModalDeleteAsset_ref"
    />
  </div>
</template>

<script>
import ModalCreate from "../../components/modal/increaseAssetModal/ModalCreate.vue";
import ModalDeleteAsset from "../../components/modal/ModalDeleteAsset.vue";
import BaseLoading from "../../components/common/BaseLoading.vue";
import axios from "axios";

export default {
  components: {
    ModalDeleteAsset,
    ModalCreate,
    BaseLoading,
  },
  props: {
    isFilterMenu: Boolean,
  },
  data() {
    return {
      listAsset: [],
      asset: {
        assetId: null,
        assetCode: null,
        assetName: null,
        departmentId: null,
        assetTypeId: null,
        timeUse: null,
        wearRate: null,
        originalPrice: null,
        wearValue: null,
      },
      listDepartment: [],
      listAssetType: [],
      formMode: "",
      alerMsg: "",
      assetIdUpdate: null,
      listSelectRow: [],
      listAssetId: [],
      inputSearch: "",
      showSuccess: false,
      isError: false,
      getSuccess: true,
      getEmty: false,
      totalPrice: 0,
      amountAsset: 0,
      showWarning: false,
      paging: {
        amountPage: 1,
        pageNumber: 1,
        recordNumber: 50,
        totalRecord: 0,
        startRecord: 0,
        endRecord: 0,
      },
      showMenuFilter: true,
      showMenuType: false,
      showMenuDepartment: true,
      comboxFilter: {
        idDepartment: null,
        idType: null,
        allAsset: "tất cả",
      },
      viewAll:true
    };
  },
  methods: {
    msgAlert(text, value) {
      this.showWarning = value;
      this.alerMsg = text;
      var res = this;

      setTimeout(() => {
        res.showWarning = false;
      }, 3000);
    },
    //todo gửi option tìm kiếm lên combobox tại header
    sendOption(text) {
      if (text == "Tất cả") {
        this.getAsset("filter");
        this.comboxFilter.idDepartment = "";
        this.comboxFilter.idType = "";
      }
      this.$emit("allAsset", text);
    },
    /**
     * Gửi request GET tới API
     * Author: TVThinh (12/5/2021)
     */
    // async getAsset(text, idDepartment, idType) {
    //   // xử lý filter trên thanh combobox
    //   if (idDepartment == undefined || idDepartment == "")
    //     this.comboxFilter.idDepartment = "";
    //   else {
    //     this.sendOption("Phòng ban"); // gửi dòng chữ 'phòng ban' hiển thị lên combobox
    //     this.comboxFilter.idDepartment = idDepartment;
    //   }
    //   if (idType == undefined || idType == "") this.comboxFilter.idType = "";
    //   else {
    //     this.sendOption("Loại tài sản");
    //     this.comboxFilter.idType = idType;
    //   }

    //   if (text == "filter") this.paging.pageNumber = 1;

    //   var res = this;
    //   this.listSelectRow = [];

    //   res.$refs.BaseLoading_ref.show();
    //   this.getEmty = false;
    //   this.amountAsset = 0;
    //   this.totalPrice = 0;

    //   if (document.getElementById("ctxMenu"))
    //     // nếu context menu vẫn hiện
    //     document.getElementById("ctxMenu").style.display = "none";

    //   await axios
    //     .get(
    //       "https://localhost:44382/api/v1/Assets/Filter/?input=" +
    //         res.inputSearch +
    //         "&recordAmount=" +
    //         this.paging.recordNumber +
    //         "&pageNumber=" +
    //         this.paging.pageNumber +
    //         "&departmentId=" +
    //         this.comboxFilter.idDepartment +
    //         "&typeId=" +
    //         this.comboxFilter.idType
    //     )
    //     .then((response) => {
    //       res.listAsset = response.data.data;

    //       if (res.listAsset.length == 0) {
    //         res.getEmty = true;
    //       }

    //       res.$refs.BaseLoading_ref.hide();
    //       res.listAssetId = [];
    //       res.paging.amountPage = response.data.totalPage;

    //       res.listAsset.forEach((element) => {
    //         // duyệt qua tất cả các bản ghi
    //         res.listAssetId.push(element.assetId); // push tất cả id tài sản vào mảng
    //         res.amountAsset++; // đếm tổng số bản ghi
    //         if (element.originalPrice != null) {
    //           res.totalPrice += parseInt(element.originalPrice); // tính tổng nguyên giá
    //         }
    //       });
    //     })
    //     .catch((error) => {
    //       console.log(error);
    //       setTimeout(() => {
    //         res.$refs.BaseLoading_ref.hide(); // tắt dialog loading
    //         res.getEmty = true; // b
    //       }, 4000);
    //     });
    // },

    async getAsset() {
      await axios.get("http://localhost:56697/api/v1/");
    },

    /// todo hiển thị dialog thêm
    showDialog(text, Id) {
      document.getElementById("ctxMenu").style.display = "none";
      if (text == "insert") {
        this.formMode = "insert";
        this.alerMsg = "Thêm mới thành công";
      } else {
        this.formMode = "update";
        this.alerMsg = "Cập nhật thành công";
        this.assetIdUpdate = Id;
      }
      setTimeout(() => {
        this.$refs.ModalCreateAsset_ref.show();
      }, 300);
      // debugger; // eslint-disable-line no-debugger
    },
    /// todo lấy ra các phòng ban
    async getDepartment() {
      var res = this;
      await axios
        .get("https://localhost:44382/api/v1/Departments")
        .then((response) => {
          res.listDepartment = response.data.data;
        })
        .catch((error) => {
          this.errorMessage = error.message;
          console.error("GET Department Failed: ", error.message);
        });
    },

    ///  todo lấy ra các loại tài sản
    async getAssetType() {
      var res = this;
      await axios
        .get("https://localhost:44382/api/v1/AssetTypes")
        .then((response) => {
          res.listAssetType = response.data.data;
        })
        .catch((error) => {
          this.errorMessage = error.message;
          console.error("GET AssetType Failed: ", error.message);
        });
    },

    // todo tải lại dữ liệu
    reload(value) {
      if (value == true) {
        this.getAsset("");
        this.showSuccess = true;
        setTimeout(() => {
          this.showSuccess = false;
        }, 6000);
      }
    },

    //todo hiển thị form xác nhận xóa
    showDeleteDialog(text) {
      // var res = this
      this.alerMsg = "Xóa thành công!";
      if (text != "inRow" && this.listSelectRow.length == 0) {
        this.showWarning = true;
        this.alerMsg = "Vui lòng chọn bản ghi";
      } else {
        this.$refs.ModalDeleteAsset_ref.show();
        this.showWarning = false;
      }
      setTimeout(() => {
        this.showWarning = false;
      }, 3000);
    },

    //  select hàng, nếu hàng đã được select thì xóa khỏi mẩng listSelectRow, và ngược lại
    selectRow(id, event) {
      document.getElementById("ctxMenu").style.display = "none";
      if (event.ctrlKey == false && event.shiftKey == false) {
        this.listSelectRow = [];
        this.listSelectRow.push(id);
      } else if (event.shiftKey) {
        var idFirst = this.listSelectRow[0];
        this.listSelectRow = [];
        this.listSelectRow.push(idFirst);

        // vị trí đầu tiên trong mảng listSelectRow
        var idStart = this.listSelectRow[0];
        var indexStart = this.listAssetId.indexOf(idStart);

        var indexEnd = this.listAssetId.indexOf(id);
        if (indexStart > indexEnd) {
          indexStart--;
          var tem = indexStart;
          indexStart = indexEnd;
          indexEnd = tem;
        } else {
          indexStart++;
        }
        for (var i = indexStart; i <= indexEnd; i++) {
          this.listSelectRow.push(this.listAssetId[i]);
        }
      } else if (event.ctrlKey) {
        var index = this.listSelectRow.indexOf(id);
        if (index > -1) {
          this.listSelectRow.splice(index, 1);
        } else {
          this.listSelectRow.push(id);
        }
      }
    },

    // kiểm tra hàng đã được select hay chưa
    selectedRow(id) {
      if (this.listSelectRow.indexOf(id) > -1) return true;
      else return false;
    },

    // todo xử lý sự kiện mũi tên lên xuống để select row
    processkey() {
      var res = this;
      document.addEventListener("keydown", function (e) {
        var len1 = res.listSelectRow.length; // số phần tử của mảng listSelectRow
        var len2 = res.listAssetId.length; //số phần tử của mảng listAssetId
        if (res.$refs.ModalCreateAsset_ref.isActive == false) {
          switch (e.keyCode) {
            case 38:
              {
                //up arrow
                if (
                  len1 == 0 ||
                  res.listAssetId.indexOf(res.listSelectRow[0]) == 0
                ) {
                  res.listSelectRow = [];
                  res.listSelectRow.push(res.listAssetId[len2 - 1]);
                } else if (len1 > 0) {
                  //res.listSelectRow = [];
                  var indexIdFirst = res.listAssetId.indexOf(
                    res.listSelectRow[0]
                  );
                  res.listSelectRow = [];
                  res.listSelectRow.push(res.listAssetId[indexIdFirst - 1]);
                }
              }
              break;
            case 40:
              {
                // //down arrow
                if (
                  len1 == 0 ||
                  res.listAssetId.indexOf(res.listSelectRow[len1 - 1]) ==
                    len2 - 1
                ) {
                  res.listSelectRow = [];
                  res.listSelectRow.push(res.listAssetId[0]);
                } else if (len1 > 0) {
                  //res.listSelectRow = [];
                  var indexIdLast = res.listAssetId.indexOf(
                    res.listSelectRow[len1 - 1]
                  );
                  res.listSelectRow = [];
                  res.listSelectRow.push(res.listAssetId[indexIdLast + 1]);
                }
              }
              break;
            case 37:
              {
                res.backPage();
              }
              break;
            case 39:
              {
                res.nextPage();
              }
              break;
            default: {
              return true;
            }
          }
        }
      });
    },

    // todo hiện và thao tác với context menu
    showContexMenu(id, e) {
      this.listSelectRow = [];
      this.listSelectRow.push(id);
      var ctx = document.getElementById("ctxMenu");
      ctx.style.display = "block";
      ctx.style.top = (e.screenY - 70).toFixed() + "px";
      ctx.style.left = e.screenX.toFixed() + "px";
    },

    // todo chuyển đến trang sau
    nextPage() {
      if (this.paging.pageNumber < this.paging.amountPage) {
        this.paging.pageNumber++;
        this.getAsset();
      }
    },
    // todo chuyển đến trang trước
    backPage() {
      if (this.paging.pageNumber > 1) {
        this.paging.pageNumber--;
        this.getAsset();
      }
    },

    // todo thay đổi khung nhìn
    changeVisual(text)
    {
      if(text == 'all')
      {
        this.viewAll = true
        document.getElementsByClassName('option-full')[0].style.background='#f5f5f5'
        document.getElementsByClassName('option-detail')[0].style.background='white'
        
      }
      else {
        this.viewAll = false
        document.getElementsByClassName('option-full')[0].style.background='white'
        document.getElementsByClassName('option-detail')[0].style.background='#f5f5f5'
        
      }
    }
  },
  filters: {
    // todo định dạng kiểu tiền tệ cho nguyên giá
    formatMoney: function (money) {
      if (money != null)
        var num = money.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
      else return "0";
      return num;
    },
  },
  watch: {},
  created() {
    //this.getAsset();
    this.processkey();
  },
  mounted() {
    this.getDepartment();
    this.getAssetType();
    this.getAsset();
  },
};
</script>


<style lang='scss' scoped>
#assetPopup {
  padding: 0 40px;
  height: 80px;
  background-color: #b1fdb3;
  color: black;
  font-size: 16px;
  border-left: 10px solid #e2f2ff;
  display: flex;
  padding-left: 26px;
  align-items: center;
  position: absolute;
  right: -500px;
  bottom: 80px;
  transition: right 1s;
}
#ctxMenu {
  background-color: white;
  position: fixed;
  z-index: 100;
  // display: none;
}

.ctx-menu {
  font-size: 14px;
  box-shadow: 2px 2px 4px grey;
  display: none;

  .ctx-menu-item {
    padding: 8px 34px 8px 18px;

    &:hover {
      background-color: #eaeaea;
      cursor: pointer;
    }
  }
}
//TODO DATETIME theo định dạng d/m/y, bỏ chọn thời gian
//TODO Sử dụng được phím tab trong nhập liệu
//TODO Thay đổi font chữ của datetimepicker
//TODO Thêm khách hàng thành công;
#loadBar {
  width: 0;
  height: 3px;
  background-color: #00abfe;
  transition: 0.5s;
  margin-top: 8px;
  display: none;
  position: absolute;
}

// .content-nav {
//     position: relative;
// }

// .content-nav  :after {
//     content: '';
//     display: block;
//     border-bottom: 3px solid #00abfe;
//     width: 0;
//     position: absolute;
//     left: 0;
//     -webkit-transition: 1s ease;
//     transition: 1s ease;
// }

// .content-nav :hover:after {
//     width: 100%;
// }

.content-nav {
  height: 70px;
  padding: 16px;
  box-sizing: border-box;

  .features-pane {
    display: flex;
    position: relative;
    align-items: center;
    margin: 10px 0;
    // width: 100%;
    padding-right: 10px;
    font-size: 14px;

    input {
      height: 34px;
      width: 320px;
      box-sizing: border-box;
    }
    .btn {
      height: 34px;
    }

    .btn-search {
      margin-left: 10px;
    }

    .features-pane-right {
      position: absolute;
      right: 0;

      // background-color:lightsalmon;
      display: flex;
      .features-pane-item {
        margin: 0px 5px;
      }
    }

    .features-pane-left {
      display: flex;
      align-items: center;
      .icon-search {
        position: relative;
        right: 45px;

        &:hover {
          cursor: pointer;
          //TODO sau này thêm action tìm kiếm vào đây
        }
      }
    }
  }
}

.content-grid {
  margin: 16px 16px 16px 16px;
  height: calc(100% - 149px);
  position: relative;
  overflow: auto;
  box-sizing: border-box;
}

.not-active {
  display: none;
}

.features-box {
  display: flex;
  align-items: center;
  visibility: hidden;
  .table-icon {
    width: 15px;
    height: 15px;
    margin: 0 4px;
    &:hover {
      cursor: pointer;
    }
  }
}

.table-summary {
  font-size: 14px;
  font-family: GoogleSans-Medium;
  display: flex;
  position: relative;
  margin: 18px 16px 0 16px;
  height: 45px;

  .price-number {
    position: absolute;
    //TODO sẽ phải sửa lại cái này cho chuẩn với cột nguyên giá
    right: 120px;
  }
}
.content {
  height: calc(100vh - 60px);
  width: calc(100% - 200px);
  margin-left: 210px;
  transition: all 0.25s;
  background-color: white;
  user-select: none;
}

.content-nav {
  padding-top: 26px;
  padding-bottom: 6px;
}
.content-nav .features-pane {
  margin: 0px;
}
.div-container {
  position: relative;
}
.table-summary {
  font-size: 14px;
  font-family: GoogleSans-Medium;
  display: flex;
  /* position: relative; */
  position: absolute;
  bottom: 0px;
  /* margin: 18px 16px 0 16px; */
  height: 63px;
}
.content {
  height: calc(100vh - 68px);
  width: calc(100% - 200px);
  margin-left: 210px;
  transition: all 0.25s;
}

.content-grid {
  margin: 16px 16px 16px 16px;
  height: calc(100% - 165px);
  /* height: 100%; */
  position: relative;
  overflow: auto;
  box-sizing: border-box;
}
.table-summary {
  font-size: 14px;
  font-family: GoogleSans-Medium;
  display: flex;
  /* position: relative; */
  width: 100%;
  position: absolute;
  bottom: 0px;
  /* align-items: center; */
  /* margin: 18px 16px 0 16px; */
  height: 63px;
  padding: 18px 16px;
  box-sizing: border-box;
}
.table-summary .summary {
  height: 100%;
  width: 100%;
  display: flex;
  align-items: center;
}

.content .div-container {
  height: 100%;
}

.content {
  position: relative;
  margin-left: 0;
  box-sizing: border-box;
  width: 100%;
  height: calc(100% - 60px);
}

table tr th {
  white-space: nowrap;
  cursor: auto !important;
}

table tbody tr {
  cursor: pointer;
}

.selected-row {
  background: #a1adb3 !important;
}

.v-sheet.v-alert {
  position: absolute;
  z-index: 2;
  right: 0px;

  animation-name: alert;
  width: 0px;
  animation-duration: 3s;
  white-space: nowrap;
  padding: 16px 0px;
  bottom: 0px;
}

@keyframes alert {
  0% {
    width: 0px;
  }
  50% {
    width: 220px;
    padding: 16px;
  }
  100% {
    width: 0px;
    padding: 16px 0px;
  }
}

table tbody tr td {
  font-family: "GoogleSans";
}
.summary {
  display: flex;
}

#ctxMenu {
  position: relative;
  width: 152px;
  position: fixed !important;
}
#required-choose {
  display: block !important;
  margin-right: 8px;
}

.text-pagebumber::-webkit-outer-spin-button,
.text-pagebumber::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

.btn-move-page {
  border-radius: 4px;
}

.p-number,
.btn-move-page {
  height: 34px;
  width: 34px;
  margin: 0 8px 0 8px;
  text-align: center;
  background-repeat: no-repeat;
  background-size: contain;
  background-size: 24px;
  background-position: center;
}

.p-number:hover,
.btn-move-page:hover {
  background-color: #bbbbbb;
  cursor: pointer;
}

.p-number {
  border-radius: 50%;
  border: 1px solid #bbbbbb;
  line-height: 34px;
}

.btn-first-page {
  background-image: url("../../assets/icon/btn-firstpage.svg");
}

.prev-page {
  background: url(../../assets/icon/common-icon.png) no-repeat -700px -124px;
}

.paging-toolbar {
  height: 100%;
  display: flex;
  margin-left: calc((100% - 752px) / 2);
}

.paging-toolbar .leftchild {
  height: 100%;
  min-width: 330px;
  display: flex;
  align-items: center;
}

.paging-toolbar .leftchild .p-button {
  height: 24px;
  width: 24px;
  border: 1px solid #d0d0d0;
  border-radius: 3px;
  background-color: #fff;
  margin-right: 2px;
}

.paging-toolbar .leftchild .first-page {
  /* background: url(/QLCH/resources/images/common-icon.png) no-repeat -649px -124px;
    background: url(../../icon/common-icon.png) no-repeat;
    background-repeat: no-repeat;
    background-size: contain;
    background-position: center; */
  background: url(../../assets/icon/common-icon.png) no-repeat -649px -124px;
  width: 24px;
  height: 24px;
  background-color: #fff;
  border-radius: 3px;
  cursor: pointer;
}

.text-pagebumber,
.select-quantitypage {
  border-radius: 3px;
  text-align: center;
}

.text-pagebumber {
  width: 36px;
  height: 16px;
  padding: 4px 6px;
  border: 1px solid #d0d0d0;
  box-sizing: border-box;
  height: 24px;
}

.refresh {
  background: url(../../assets/icon/common-icon.png) no-repeat -849px -124px;
  border: 1px solid #6b6f9d !important;
}

.leftchild .last-page {
  background: url(../../assets/icon/common-icon.png) no-repeat -798px -124px;
}

.leftchild .next-page {
  background: url(../../assets/icon/common-icon.png) no-repeat -749px -124px;
}
.select-quantitypage {
  width: 46px;
  height: 26px;
  border: 1px solid #d0d0d0;
  outline: none !important;
}
.text-pagebumber,
.select-quantitypage {
  border-radius: 3px;
  text-align: center;
}

.select-quantitypage {
  appearance: none;
  background: url(../../assets/icon/arrow-down-line.png) no-repeat;
  background-position: 29px 11px;
  padding-left: 4px;
}
::-webkit-scrollbar {
  display: none;
}

.filter-menu {
  height: 80px;
  width: 102px;
  display: flex;
  flex-direction: column;
  position: absolute;
  top: -14px;
  z-index: 100;
  border-radius: 2px;
  right: 214px;
  font-size: 14px;
  box-shadow: 2px 2px 4px grey;
  background: white;
  cursor: pointer;
}

.filter-menu .item-type {
}
.filter-menu .item-filter {
  height: 50%;
  display: flex;
  align-items: center;
}

.filter-menu .menu-child {
  width: 150px;
  height: 400px;
  display: flex;
  flex-direction: column;
  overflow: auto;
  box-shadow: 2px 2px 4px grey;
}
.filter-menu .menu-child .item {
  height: 40px;
  display: flex;
  align-items: center;
  background: white;
  padding: 10px 5px;
  text-align: center;
}

.filter-menu .menu-container {
  position: absolute;
  left: 102px;
  top: 15px;
}

.filter-menu .text {
  padding: 0px 5px;
}
.filter-menu .item:hover {
  background: #eaeaea;
}

.filter-menu .menu-department {
  top: 40px;
}
.filter-menu .menu-child {
  display: none;
}

.item-filter:hover .menu-child {
  display: block;
}
.menu-child:hover {
  display: block;
}
table {
  position: relative;
}

.loading-emty {
  font-size: 24px;
  font-style: italic;
  white-space: nowrap;
  position: absolute;
  left: calc((100% - 180px) / 2);
  margin-top: 21px;
}

.v-icon {
  background-image: url("../../assets/icon/add.png") !important;
  background-size: 16px;
  background-repeat: no-repeat;
  background-position: center;
  height: 28px;
  width: 28px;
}
.features-pane-top {
  height: 34px;
  background: green;
  width: 100%;
}

.content-nav {
  padding: 10px;
  height: auto;
  margin: 0 16px;
}
.content-nav .features-pane {
  margin: 0px;
}
.content-grid {
  margin: 0 16px;
}
.panel-feature-top {
  height: 54px;
  position: relative;
  padding: 10px 16px;
}
.panel-feature-top .panel-feature {
  height: 100%;
  display: flex;
}
.panel-feature-top .panel-feature .feature-left {
  height: 100%;
  display: flex;
  align-items: center;
  padding-left: 8px;
  font-weight: 700;
}
.panel-feature-top .panel-feature .feature-right {
  position: absolute;
  right: 0;
  display: flex;
  align-items: center;
}
.button-refresh {
  padding-left: 20px;
}
.button-refresh .icon {
  height: 20px;
  width: 20px;
  background-image: url("../../assets/icon/refresh1.png");
  background-repeat: no-repeat;
  background-size: contain;
}

.input-search {
  padding: 8px 46px 8px 43px;
  font-size: 12px;
}

.content-nav .features-pane .features-pane-left .icon-search {
  position: absolute;
  /* right: 45px; */
  left: 0px;
}
.content-nav .features-pane .features-pane-left {
  position: relative;
}
.content-nav .features-pane input {
  width: 236px;
}
.content .div-container {
  background: #f5f6fa;
}
.content-nav {
  background: white;
}
table thead tr th {
  background: #f5f6fa;
}
table tbody {
  background: white;
}
.feature-right .button-option-view {
  height: 34px;
  width: 68px;
  margin-left: 10px;
  box-sizing: border-box;
  background: white;
  border-radius: 3px;
  padding-left: 14px;
  display: flex;
  align-items: center;
}
.panel-feature-top .panel-feature {
  height: 100%;
  display: flex;
  position: relative;
}

.panel-feature-top .panel-feature .icon-arrow {
  background-image: url("../../assets/UI/Icon/i_arrow_up_small.svg");
  background-repeat: no-repeat;
  background-size: 9px;
  transform: rotate(0deg);
  background-position: center;
  width: 10px;
  height: 10px;
  opacity: 0.6;
  margin-left: 12px;
}

.feature-right .button-option-view .menu-option-view {
  height: 52px;
  width: 68px;
  margin-left: 10px;
  box-sizing: border-box;
  background: white;
  border-radius: 3px;
  display: flex;
  align-items: center;
  position: absolute;
  right: 0;
  display: flex;
  flex-direction: column;
  top: 35px;
  z-index: 1;
  box-shadow: 2px 2px 10px #c3c3c3;
  padding-top: 5px;
}
 .icon-view-all {
  height: 20px;
  width: 20px;
  background-image: url("../../assets/icon/view-full.png");
  background-repeat: no-repeat;
  background-size: contain;
  position: absolute;
  right: -9px;
}

.icon-view-detail {
  height: 20px;
  width: 20px;
  background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAZCAYAAAA14t7uAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEuSURBVEhL7ZIxi4MwGIbvX1fo5OLWycmCqIsILgodOhREBN0cFBwchDrUqVAQKqL1PSKh11ykl+EObvCBd8v78OVLPvBHrOInnPjxeOB+v6NtW6EMw0CbLIx4HEekaQrTNKFpmlBc10Xf99TwBTfx7XZDlmVIkkQodV3TJgsjnqYJ1+sVZVmiKAqhnM/neX3fYcTkAJH6vg/HcYQSBMHinrlVdF2HpmnmK4qErG4JTpznOWzbhmEYQvE8jzZZOPHhcICiKLAsa/Hqr1FVFdvtljZZODGZQNd1XC6X+T+/SxiGkCSJNlkWxeSK5Hf8RBzH/0S83+9RVdUsf5fT6YTNZkObLJz4eDzOj0fkr6+/lN1uB1mWaZOFE5NJiZxMLpIoimiThRP/Fqv4ySqmAJ+v/hYupF8OGwAAAABJRU5ErkJggg==);
  background-repeat: no-repeat;
  background-size: 28px 28px;
  background-position: center;
  position: absolute;
  right: -9px;
}
.panel-feature-top .panel-feature .icon-chosen {
  height: 20px;
  width: 20px;
  background-image: url("../../assets/icon/chosen-view.png");
  background-repeat: no-repeat;
  background-size: contain;
}
.panel-feature-top .panel-feature .item-option {
  display: flex;
  cursor: pointer;
}
.panel-feature-top .panel-feature .icon-view-detail{
    position: absolute;
    right: -9px;
}
.panel-feature-top .panel-feature .item-option {
    display: flex;
    width: 40px;
    position: relative;
}

 .option-detail{
  margin-top:5px;
}

.panel-feature-top .panel-feature .item-option{
    width: 100%;
    height: 50%;
    position: relative;
}
.panel-feature-top .panel-feature .icon-chosen{
    height: 20px;
    width: 20px;
    background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAYAAADEtGw7AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAKpSURBVEhL1ZTfS1NhGMf7N7oMoruwbsIiCKIUL4RCCOwuvFBKKA203zbN8EdFJKWUXRTO1mJz0zZ/m87MmmNay9zS1jbd3OZ+nZ3t+tv7vLHDzs4xIojo4nPxvs/zfN/3fJ/nPTuy2Sz+Bv9WOJ5OIpiMIiUKqnE1thVOpFMY9znR7RrG1Q99qJt7iib7C/Quj8EWcKnW5KMqHEhEcMuhx3HLTZSP3EaNrQcN889QNf0QZdYWvv/g4xBCyS1FbQ6FsD8RxsmxNhwyX0aPawTO0Cq8sRDW2WFrsQ3Ygx7cWTRhn/EiamcfY4NZVKhBKITpk3frz2HCt8jtKIwTMea5YXUORYZ6tDpeqebIhLUr09ijr8XA2rwsSY2UmGZ2vMYBUwOGvQ5FXCZ8drYHx5h/+Xv5JNICdB4bvzGtXeHv3O8Op1GRKwkn2SgdGbqGG/Z+vs5kMtgSEtxfgd2OGqVZ0KHYfAnvgys8h/pxfq4X1TPdPCenRUjC7qifF9E40VrMiNznM2+6YFx9h+aFlyixaNDnnpZENlMxtDsNOD15D774piRKSMLU3cODV9C5OMDXIruxI/SVTUg79rMJKBtuwfOVKcSEnzbkamjG6fBIKi7tEzKPK9nJlZN3pTW9tCn/EhrZDNNNC4vd0QAqxjpw3a6V7RMy4S7W5SJDHd6uL0t7JE4zTH7n5wqiiH73DA6yedd7ZmUxQibsZz5VjLej1NoMz1ZAllgI2VRi1aDa9oj/RwrjMmGCCuiBkKfzG19ksRzjrKnF5kYctTThU9irmqMQJtzstqXsNrt0NbzjTz6PwsQeDVlVPtKKndoqnJroxLdYUFGbQ1WYiApx3F8axInRNvZfqMdewwU+HdRcOkitJp9thfMhD5cjPkUDf8VvCf8J/5twFj8AZJXbCDaq+F8AAAAASUVORK5CYII=);
    background-repeat: no-repeat;
    background-size: contain;
    position: absolute;
    left: 12px;
}
.panel-feature-top .panel-feature .icon-view-all {
    right: 4px;
}
.panel-feature-top .panel-feature .icon-view-detail {
    position: absolute;
    right: 4px;
}

.menu-option-view .item-option:hover{
  background: #f5f5f5;
}
.menu-option-view .option-full{
  background: #f5f5f5;
}
.panel-feature-top .panel-feature .icon-view {
  position: inherit;
}
.menu-option-view{
  display: none!important;;
}
.button-option-view:hover .menu-option-view{
  display: block!important;
}
.content-nav .features-pane input{
    border-radius: 2px;
}
.icon-print{
  background-image: url("../../assets/icon/print-removebg.png");
  background-repeat: no-repeat;
    background-position: center;
    opacity: 0.8;
}
.content-nav .features-pane .btn {
    padding: 16px!important;
    /* width: 34px; */
}
.icon-more{
  background-image: url("../../assets/icon/option-more.png");
background-repeat: no-repeat;
    background-position: center;
}
.content-nav {
    background: white;
     border: 1px solid #d0cece;
    box-shadow: 0px 0px 20px #bbb6b6;
    box-sizing: border-box;
    border-bottom:none ;
}
.content-grid {
    margin: 0 16px;
    box-sizing: border-box;
    border: 1px solid #d0cece;
    box-shadow: 0px 12px 20px #bbb6b6;
}
table tbody tr td{
  border-right: none;
}

</style>