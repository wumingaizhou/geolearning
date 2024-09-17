#### git常见操作

git clone [url]

git checkout -b 'name'  新建并且切换分支

git checkout name 切换分支

git add . 

git commit -m '描述'

git push origin name

git pull 拉取远端，切换到主分支的时候要拉起远端，更新代码



git remote add origin 仓库地址



撤销commit，不撤销 git add ：git reset --soft HEAD~1 ，1 也就是撤销上一次commit，如果进行了 2 次commit，都想撤回，就使用 ~2



撤销 commit ，并且撤销 git add . 操作，不撤销修改代码：git reset --mixed HEAD^